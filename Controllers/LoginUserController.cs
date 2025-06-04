using Hoved_Opgave_Datamatiker.Models.LoginUser;
using Hoved_Opgave_Datamatiker.Models; // Namespace for din Customer-model
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Hoved_Opgave_Datamatiker.DBContext;

namespace Hoved_Opgave_Datamatiker.Controllers
{
    /// <summary>
    /// Controller til håndtering af login og generering af JWT tokens.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginUserController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AppDBContext _customerContext;

        /// <summary>
        /// Konstruktor for LoginUserController.
        /// </summary>
        /// <param name="signInManager">SignInManager til at validere brugerlogin.</param>
        /// <param name="userManager">UserManager til håndtering af brugerdata.</param>
        /// <param name="configuration">App-konfiguration (JWT indstillinger).</param>
        /// <param name="customerContext">Databasekontekst til kundeoplysninger.</param>
        public LoginUserController(SignInManager<IdentityUser> signInManager,
                                   UserManager<IdentityUser> userManager,
                                   IConfiguration configuration,
                                   AppDBContext customerContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _customerContext = customerContext;
        }

        /// <summary>
        /// Logger brugeren ind og returnerer et JWT-token sammen med CustomerId.
        /// </summary>
        /// <param name="model">Login-modellen med email, kodeord og RememberMe.</param>
        /// <returns>JWT-token og kundens ID hvis tilgængelig.</returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { error = "Ugyldigt loginforsøg." });

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
                return Unauthorized(new { error = "Ugyldigt loginforsøg." });

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(3),
                claims: claims,
                signingCredentials: creds
            );

            var customer = await _customerContext.Customers.SingleOrDefaultAsync(c => c.Email == model.Email);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                customerId = customer?.CustomerId
            });
        }
    }
}
