using Hoved_Opgave_Datamatiker.Models.LoginUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hoved_Opgave_Datamatiker.Controllers
{
    /// <summary>
    /// Controller til håndtering af registrering af nye brugere.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterUserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Konstruktor som modtager UserManager og RoleManager via dependency injection.
        /// </summary>
        public RegisterUserController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Registrerer en ny bruger med e-mail og adgangskode og tildeler rollen "Customer".
        /// </summary>
        /// <param name="model">Brugerens registreringsoplysninger.</param>
        /// <returns>Statuskode og besked om resultatet af registreringen.</returns>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Password != model.ConfirmPassword)
                return BadRequest(new { error = "Password and confirmation do not match." });

            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return BadRequest(new { error = "User with this email already exists." });

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Sikrer at rollen "Customer" findes, og opretter den hvis nødvendigt
            if (!await _roleManager.RoleExistsAsync("Customer"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            // Tildeler den nye bruger rollen "Customer"
            await _userManager.AddToRoleAsync(user, "Customer");

            return Ok(new { message = "User registered and assigned 'Customer' role successfully.", userId = user.Id });
        }

        /// <summary>
        /// Henter en liste over alle registrerede brugere med deres ID og e-mail.
        /// </summary>
        /// <returns>Liste med brugere.</returns>
        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            var users = _userManager.Users.Select(u => new { u.Id, u.Email }).ToList();
            return Ok(users);
        }

    }
}
