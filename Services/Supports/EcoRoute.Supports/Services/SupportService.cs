using AutoMapper;
using EcoRoute.Supports.Context;
using EcoRoute.Supports.Dtos.SupportTicketDto;
using EcoRoute.Supports.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace EcoRoute.Supports.Services
{
    public class SupportService : ISupportService
    {
        private readonly SupportsContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public SupportService(SupportsContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task<Guid> CreateAsync(CreateSupportTicketDto dto)
        {
            var ticket = _mapper.Map<SupportTicket>(dto);

            if (dto.Attachment != null)
            {
                var fileName = $"{Guid.NewGuid()}_{dto.Attachment.FileName}";
                var filePath = Path.Combine(_env.WebRootPath, "attachments", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Attachment.CopyToAsync(stream);
                }

                ticket.AttachmentPath = $"/attachments/{fileName}";
            }

            _context.SupportTickets.Add(ticket);
            await _context.SaveChangesAsync();

            return ticket.Id;
        }

        public async Task<List<ResultSupportTicketDto>> GetAllAsync()
        {
            var tickets = await _context.SupportTickets
                .Include(x => x.Responses)
                .ToListAsync();

            return _mapper.Map<List<ResultSupportTicketDto>>(tickets);
        }

        public async Task<GetByIdSupportTicketDto?> GetByIdAsync(Guid id)
        {
            var ticket = await _context.SupportTickets
                .Include(x => x.Responses)
                .FirstOrDefaultAsync(x => x.Id == id);

            return ticket == null ? null : _mapper.Map<GetByIdSupportTicketDto>(ticket);
        }
    }
}
