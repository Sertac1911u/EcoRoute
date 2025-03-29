using EcoRoute.IdentityServer.Dtos;
using EcoRoute.IdentityServer.Models;
using EcoRoute.IdentityServer.Tools;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EcoRoute.IdentityServer.Controllers
{
    [EnableCors("AllowBlazorClient")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public LoginsController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(UserLoginDto userLoginDto)
        {
            var result = await _signInManager.PasswordSignInAsync(userLoginDto.Username, userLoginDto.Password, false, false);
            var user = await _userManager.FindByNameAsync(userLoginDto.Username);

            if (result.Succeeded && user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var model = new GetCheckAppUserViewModel
                {
                    Username = user.UserName,
                    Id = user.Id
                };

                var token = JwtTokenGenerator.GenerateToken(model, userRoles);
                return Ok(token);
            }
            else
            {
                return Unauthorized(new { message = "Access Denied" });
            }
        }
    }
}
