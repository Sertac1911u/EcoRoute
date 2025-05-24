using EcoRoute.IdentityServer.Dtos;
using EcoRoute.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EcoRoute.IdentityServer.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("GetAllUserList")]
        public async Task<IActionResult> GetAllUserList()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<ResultUserDto>();

            foreach (var user in users)
            {
                // Rol burada çekiliyor
                var roles = await _userManager.GetRolesAsync(user);

                // Eğer kullanıcı SuperAdmin ise listeye ekleme
                if (roles.Contains("SuperAdmin"))
                    continue;

                userDtos.Add(new ResultUserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Name = user.Name,
                    Surname = user.Surname,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList(),
                    CreateDate = user.CreateDate,     
                    LastLoginDate = user.LastLoginDate 
                });
            }

            return Ok(userDtos);
        }


        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID claim not found.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.Name,
                user.PhoneNumber,
                user.Surname
            });
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return BadRequest(updateResult.Errors);

            // 🟨 ROL güncelle
            if (!string.IsNullOrWhiteSpace(model.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            // 🔐 ŞİFRE güncelle (yeni şifre girildiyse)
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                // Kullanıcının önceki şifresi varsa kaldır
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.Password);

                if (!passwordResult.Succeeded)
                    return BadRequest(passwordResult.Errors);
            }

            return Ok("Kullanıcı başarıyla güncellendi.");
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Name = dto.Name,
                Surname = dto.Surname,
                PhoneNumber = dto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, dto.Role);
            return Ok("Kullanıcı başarıyla oluşturuldu.");
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("SuperAdmin"))
                return BadRequest("SuperAdmin kullanıcı silinemez.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Kullanıcı başarıyla silindi.");
        }

        [HttpGet("getnamesbyids")]
        public async Task<IActionResult> GetUserNamesByIds([FromQuery] List<string> id)
        {
            var users = await _userManager.Users
                .Where(u => id.Contains(u.Id))
                .ToListAsync();

            var result = users.ToDictionary(
                u => u.Id,
                u => u.UserName
            );

            return Ok(result);
        }
        [HttpGet("activity")]
        public async Task<IActionResult> GetUserActivityReport()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<UserActivityReportDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault(); // çoklu rol desteği yoksa tekini al

                result.Add(new UserActivityReportDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Role = role,
                    LastLoginDate = user.LastLoginDate,
                    OperationCount = 0 // bu veriyi bir yerden topluyorsan buraya koyabilirsin
                });
            }

            return Ok(result);
        }


    }
}

