using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieAppAPI.ViewModel;
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

        public AccountController(UserManager<IdentityUser> userManager, IConfiguration iConfiguration, SignInManager<IdentityUser> signInManager)
        {

            _userManager = userManager;
            _iConfiguration = iConfiguration;
            _signInManager = signInManager;
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
                Email = signUp.Email,
            };
            var result = await _userManager.CreateAsync(signups, signUp.Password);
            result = await _userManager.AddToRoleAsync(signups, UserRole.User.ToString());

            return Ok(signups);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginVM login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
            if (!result.Succeeded)
            {
                return null;
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //new Claim(JwtRegisteredClaimNames.Aud, "User") // or "Admin" depending on the user's role
            };
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
    }
}
