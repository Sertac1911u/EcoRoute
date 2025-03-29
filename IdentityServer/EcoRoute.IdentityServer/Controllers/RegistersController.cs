using EcoRoute.IdentityServer.Dtos;
using EcoRoute.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace EcoRoute.IdentityServer.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegistersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username, 
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (!string.IsNullOrEmpty(model.Role))
            {
                var roleExists = await _roleManager.RoleExistsAsync(model.Role);
                if (!roleExists)
                {
                    return BadRequest($"Role {model.Role} does not exist.");
                }

                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return Ok("User created successfully.");
        }
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("pong");
        }

    }
}
