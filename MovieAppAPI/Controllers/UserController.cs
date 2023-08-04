using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieAppAPI.Controllers;
using MovieAppAPI.ViewModel;
using MovieAppApplication.Interface.IServices;
using MovieAppInfrastructure.Persistance;
using MovieAppInfrastructure.Persistance.Enum;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        private readonly MailController _mailController;

        public AccountController(UserManager<IdentityUser> userManager, 
        IConfiguration iConfiguration,
        SignInManager<IdentityUser> signInManager,
        IEmailService emailService
        )
        {

            _userManager = userManager;
            _iConfiguration = iConfiguration;
            _signInManager = signInManager;
            _iEmailService = emailService;
            
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
                return null;
            }
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
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
            var subject = "MovieAppLogin";
            var body = $"User {login.Email} has logged in at {DateTime.Now}.";
            _iEmailService.SendMail(subject, body);
            return Ok(jwt);
        }
           
    }
}

