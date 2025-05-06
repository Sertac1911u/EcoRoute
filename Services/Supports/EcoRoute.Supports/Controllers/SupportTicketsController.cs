using EcoRoute.Supports.Dtos.SupportTicketDto;
using EcoRoute.Supports.Dtos.TicketResponseDto;
using EcoRoute.Supports.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcoRoute.Supports.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]  
    public class SupportTicketsController : ControllerBase
    {
        private readonly ISupportService _supportService;

        public SupportTicketsController(ISupportService supportService)
        {
            _supportService = supportService;
        }

        [HttpPost]
        [Authorize(Roles = "Driver")]  
        public async Task<IActionResult> Create([FromForm] CreateSupportTicketDto dto)
        {
            if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
            {
                return Forbid("Administrators cannot create support tickets");
            }
            // Kullanıcı bilgisini alın
            dto.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            dto.UserName = User.FindFirst(ClaimTypes.Name)?.Value ?? "Bilinmeyen Kullanıcı";

            var createdTicket = await _supportService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdTicket.Id }, createdTicket.Id);
        }

        [HttpGet]
        public async Task<ActionResult<List<ResultSupportTicketDto>>> GetAll()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isManagerOrSuperAdmin = User.IsInRole("Manager") || User.IsInRole("SuperAdmin");

            var tickets = await _supportService.GetAllAsync(userId, isManagerOrSuperAdmin);
            return Ok(tickets); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetByIdSupportTicketDto>> GetById(Guid id)
        {
            var ticket = await _supportService.GetByIdAsync(id);
            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }
        [HttpPost("reply")]
        public async Task<IActionResult> AddResponse([FromForm] CreateTicketResponseDto dto)
        {
            // 1) Kullanıcı bilgilerini set et
            dto.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            dto.UserName = User.FindFirst(ClaimTypes.Name)?.Value ?? "Bilinmeyen Kullanıcı";

            // 2) Şimdi mutlaka IsStaff’ı doğru tespit edin
            dto.IsStaff = User.IsInRole("Manager")
                        || User.IsInRole("SuperAdmin");

            // 3) Servise geçin
            await _supportService.AddResponseAsync(dto);
            return NoContent();
        }


        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id mismatch");

            var success = await _supportService.UpdateStatusAsync(id, dto.Status);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/close")]
        [Authorize(Roles = "Admin,SuperAdmin")] 
        public async Task<IActionResult> CloseTicket(Guid id)
        {
            if (User.IsInRole("Driver"))
            {
                return Forbid("Drivers cannot close support tickets.");
            }
            var success = await _supportService.CloseTicketAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}