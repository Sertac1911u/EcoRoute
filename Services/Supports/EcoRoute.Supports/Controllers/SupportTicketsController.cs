using EcoRoute.Supports.Dtos.SupportTicketDto;
using EcoRoute.Supports.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoute.Supports.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportTicketsController : ControllerBase
    {
        private readonly ISupportService _supportTicketService;

        public SupportTicketsController(ISupportService supportTicketService)
        {
            _supportTicketService = supportTicketService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateSupportTicketDto dto)
        {
            var id = await _supportTicketService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _supportTicketService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _supportTicketService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }
    }
}

