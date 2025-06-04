using Hoved_Opgave_Datamatiker.Models.LoginUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Hoved_Opgave_Datamatiker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleController(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRole model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _roleManager.RoleExistsAsync(model.RoleName))
                return BadRequest($"Role '{model.RoleName}' already exists.");

            var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));

            if (result.Succeeded)
                return Ok($"Role '{model.RoleName}' created successfully.");

            return BadRequest(result.Errors);
        }

       
        [HttpPost("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] UserRole model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound("User not found");

            string role = model.RoleName; 

            // Create role if it doesn't exist
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            // Prevent duplicate role assignment
            if (await _userManager.IsInRoleAsync(user, role))
                return BadRequest($"User '{user.Email}' is already in role '{role}'.");

            var result = await _userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
                return Ok($"Role '{role}' assigned to user '{user.Email}'.");

            return BadRequest(result.Errors);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("UsersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var users = _userManager.Users.ToList();
            var response = new List<UserWithRolesDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                response.Add(new UserWithRolesDto
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return Ok(response);
        }
    }
}
