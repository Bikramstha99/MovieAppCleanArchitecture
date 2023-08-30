using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieAppAPI.Controllers;
using MovieAppAPI.ViewModel;
using MovieAppApplication.Interface.IServices;
using MovieAppDomain.Entities;
using MovieAppInfrastructure.Persistance;
using MovieAppInfrastructure.Persistance.Enum;
using Nest;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Twilio;
using static System.Net.WebRequestMethods;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;


        private readonly IConfiguration _iConfiguration;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _iEmailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MovieDbContext _movieDbContext;

        public AccountController(UserManager<IdentityUser> userManager,
        IConfiguration iConfiguration,
        SignInManager<IdentityUser> signInManager,
        IEmailService emailService,
        RoleManager<IdentityRole> roleManager,
        MovieDbContext movieDbContext
        )
        {
            _userManager = userManager;
            _iConfiguration = iConfiguration;
            _signInManager = signInManager;
            _iEmailService = emailService;
            _roleManager = roleManager;
            _movieDbContext = movieDbContext;
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp([FromForm] SignUpVM signUp)
        {
            var userExists = _userManager.FindByEmailAsync(signUp.Email).Result;
            if (userExists != null)
            {
                return BadRequest();
            }
            var signups = new IdentityUser()
            {
                UserName = signUp.UserName,
                Email = signUp.Email

            };
            var result = await _userManager.CreateAsync(signups, signUp.Password);
            result = await _userManager.AddToRoleAsync(signups, UserRole.User.ToString());

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginVM login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user.TwoFactorEnabled)
            {
                await _signInManager.SignOutAsync();
                await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);

                var tokens = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                var otpExpiration = DateTime.Now.AddMinutes(45);

                EmailServiceVM emailServiceVM = new EmailServiceVM()
                {
                    Message = "Your OTP is: ",
                    Subject = "OTP Confirmation",
                    HtmlContent = "Use the OTP to confirm your login."+tokens,
                    ReceiverEmail = login.Email

                };
                _iEmailService.SendSMTPEmail(emailServiceVM);

                var otpEntry = new TwoFactorOTP
                {
                    Email = user.Email,
                    OTP = tokens,
                    GeneratedDateTime=DateTime.Now,
                    ExpiredDateTime= otpExpiration,
                };

                _movieDbContext.TwoFactorOTP.Add(otpEntry);
                await _movieDbContext.SaveChangesAsync();

                var response = new Responses<string>
                {
                    ReceiveEmail = login.Email,
                    Status = Status.Success.ToString(),
                    Message = "OTP confirmation",
                    HttpStatus = StatusCodes.Status200OK,
                };
                return StatusCode(response.HttpStatus, response);               
            }

            // Determine the user's roles from the user's claims in the database
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userID", user.Id)
             };

            // Add the appropriate role claim based on whether the user is an admin or not
            if (userRoles.Contains("Admin"))
            {
                authClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                authClaims.Add(new Claim(ClaimTypes.Role, "User"));
            }

            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_iConfiguration["JWT:secret"]));
            var token = new JwtSecurityToken(
                issuer: _iConfiguration["JWT:validIssuer"],
                audience: _iConfiguration["JWT:validAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                );


            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(jwt);
        }
        [HttpPost("Login-2FA")]
        public async Task<IActionResult> LoginWithOTP(string code, string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                var latestOtpEntry = await _movieDbContext.TwoFactorOTP
                .Where(otp => otp.Email == user.Email && otp.ExpiredDateTime > DateTime.UtcNow)
                .OrderByDescending(otp => otp.GeneratedDateTime) // Order by GeneratedDateTime in descending order
          .      FirstOrDefaultAsync();

                if (latestOtpEntry != null && latestOtpEntry.OTP == code)
                {
                    var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim("userID", user.Id)
                         };

                    var userRoles = await _userManager.GetRolesAsync(user);

                    // Add the appropriate role claim based on whether the user is an admin or not
                    if (userRoles.Contains("Admin"))
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }
                    else
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, "User"));
                    }

                    var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_iConfiguration["JWT:secret"]));
                    var token = new JwtSecurityToken(
                        issuer: _iConfiguration["JWT:validIssuer"],
                        audience: _iConfiguration["JWT:validAudience"],
                        expires: DateTime.Now.AddDays(1),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                    );
                    var jwt = new JwtSecurityTokenHandler().WriteToken(token);                  
                    //_movieDbContext.TwoFactorOTP.Remove(latestOtpEntry);
                    //await _movieDbContext.SaveChangesAsync();

                    return Ok(jwt);
                }
                else
                {                    
                    return BadRequest("Two-factor authentication failed.");
                }
            }
            else
            {
                return BadRequest("Invalid OTP or OTP has expired.");
            } 
        }
        [HttpPost("Login-Phone")]
        public async Task<IActionResult> LoginPhone([FromForm] string phoneNumber)
        {
            var user = await _movieDbContext.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (user.TwoFactorEnabled)
            {
                var twilioSettings = _iConfiguration.GetSection("TwilioSettings");
                var accountSid = twilioSettings["AccountSid"];
                var authToken = twilioSettings["AuthToken"];
                var parameters = new List<KeyValuePair<string, string>>
             {
                new KeyValuePair<string, string>("To", phoneNumber),
                new KeyValuePair<string, string>("Channel", "sms")
            };

                var content = new FormUrlEncodedContent(parameters);

                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{accountSid}:{authToken}"));
                var authHeader = new AuthenticationHeaderValue("Basic", credentials);

                using (var httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Authorization = authHeader;

                    var response = httpClient.PostAsync("https://verify.twilio.com/v2/Services/VA5c1c990b3121b9fbc8a9b0e0aa86dd31/Verifications", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok("OTP sent successfully");
                    }
                    else
                    {
                        return BadRequest("Failed to send OTP");
                    }
                }
            }
                // Determine the user's roles from the user's claims in the database
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                 {
                new Claim(ClaimTypes.Name, phoneNumber),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userID", user.Id)
                 };
    
                // Add the appropriate role claim based on whether the user is an admin or not
                if (userRoles.Contains("Admin"))
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }
                else
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, "User"));
                }

                var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_iConfiguration["JWT:secret"]));
                var token = new JwtSecurityToken(
                    issuer: _iConfiguration["JWT:validIssuer"],
                    audience: _iConfiguration["JWT:validAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                    );


                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(jwt);           
        }

        [HttpPost("LoginPhone-2FA")]
        public async Task<IActionResult> LoginWithOTPPhone(string code, string phoneNumber)
        {
            var user = await _movieDbContext.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (user != null)
            {
                var latestOtpEntry = await _movieDbContext.TwoFactorOTP
                .Where(otp => otp.Email == user.Email && otp.ExpiredDateTime > DateTime.UtcNow)
                .OrderByDescending(otp => otp.GeneratedDateTime) // Order by GeneratedDateTime in descending order
          .FirstOrDefaultAsync();

                if (latestOtpEntry != null && latestOtpEntry.OTP == code)
                {
                    var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim("userID", user.Id)
                         };

                    var userRoles = await _userManager.GetRolesAsync(user);

                    // Add the appropriate role claim based on whether the user is an admin or not
                    if (userRoles.Contains("Admin"))
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }
                    else
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, "User"));
                    }

                    var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_iConfiguration["JWT:secret"]));
                    var token = new JwtSecurityToken(
                        issuer: _iConfiguration["JWT:validIssuer"],
                        audience: _iConfiguration["JWT:validAudience"],
                        expires: DateTime.Now.AddDays(1),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                    );
                    var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                    //_movieDbContext.TwoFactorOTP.Remove(latestOtpEntry);
                    //await _movieDbContext.SaveChangesAsync();

                    return Ok(jwt);
                }
                else
                {
                    return BadRequest("Two-factor authentication failed.");
                }
            }
            else
            {
                return BadRequest("Invalid OTP or OTP has expired.");
            }
        }
    }
}

