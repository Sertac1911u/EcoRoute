using AutoMapper;
using EcoRoute.Supports.Context;
using EcoRoute.Supports.Dtos.SupportTicketDto;
using EcoRoute.Supports.Dtos.TicketResponseDto;
using EcoRoute.Supports.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoRoute.Supports.Services
{
    public class SupportService : ISupportService
    {
        private readonly SupportsContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly ISupportNotificationService _notificationService;

        public SupportService(SupportsContext context, IMapper mapper, IWebHostEnvironment env, ISupportNotificationService notificationService)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
            _notificationService = notificationService;
        }

        public async Task<ResultSupportTicketDto> CreateAsync(CreateSupportTicketDto dto)
        {
            var ticket = _mapper.Map<SupportTicket>(dto);
            ticket.UserId = dto.UserId;
            ticket.UserName = dto.UserName;
            if (dto.Attachment != null)
            {
                try
                {
                    // ContentRootPath her zaman dolu olur
                    var uploadsFolder = Path.Combine(_env.ContentRootPath, "Uploads", "Attachments");

                    // Klasör yoksa oluştur
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Benzersiz dosya adı oluştur
                    var fileName = $"{Guid.NewGuid()}_{dto.Attachment.FileName}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // Dosyayı kaydet
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.Attachment.CopyToAsync(stream);
                    }

                    // Dosya yolunu URL olarak veritabanında sakla
                    ticket.AttachmentPath = $"/Uploads/Attachments/{fileName}";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Dosya yükleme hatası: {ex.Message}");
                    // Dosya yükleme hatası gizle ve devam et
                    ticket.AttachmentPath = null;
                }
            }

            _context.SupportTickets.Add(ticket);
            await _context.SaveChangesAsync();

            var createdTicket = await _context.SupportTickets
                .Include(x => x.Responses)
                .FirstOrDefaultAsync(x => x.Id == ticket.Id);

            var result = _mapper.Map<ResultSupportTicketDto>(createdTicket);

            // Send notification about new ticket
            await _notificationService.SendTicketCreatedNotificationAsync(result);

            return result;
        }

        public async Task<List<ResultSupportTicketDto>> GetAllAsync()
        {
            var tickets = await _context.SupportTickets
                .Include(x => x.Responses)
                .ToListAsync();

            return _mapper.Map<List<ResultSupportTicketDto>>(tickets);
        }

        public async Task<List<ResultSupportTicketDto>> GetAllAsync(string userId, bool isManagerOrSuperAdmin)
        {
            IQueryable<SupportTicket> query = _context.SupportTickets.Include(x => x.Responses);

            if (!isManagerOrSuperAdmin)
            {
                query = query.Where(x => x.UserId == userId);
            }

            var tickets = await query.ToListAsync();
            return _mapper.Map<List<ResultSupportTicketDto>>(tickets);
        }

        public async Task<GetByIdSupportTicketDto?> GetByIdAsync(Guid id)
        {
            var ticket = await _context.SupportTickets
                .Include(x => x.Responses)
                .FirstOrDefaultAsync(x => x.Id == id);

            return ticket == null ? null : _mapper.Map<GetByIdSupportTicketDto>(ticket);
        }

        public async Task AddResponseAsync(CreateTicketResponseDto dto)
        {
            var ticket = await _context.SupportTickets
                .FirstOrDefaultAsync(x => x.Id == dto.SupportTicketId);

            if (ticket == null)
                throw new Exception("Ticket not found");

            var response = new TicketResponse
            {
                Message = dto.Message,
                IsStaff = dto.IsStaff,
                SupportTicketId = dto.SupportTicketId,
                UserId = dto.UserId,
                UserName = dto.UserName
            };

            if (dto.Attachment != null)
            {
                try
                {
                    var uploadsFolder = Path.Combine(_env.ContentRootPath, "Uploads", "Attachments");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = $"{Guid.NewGuid()}_{dto.Attachment.FileName}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.Attachment.CopyToAsync(stream);
                    }

                    response.AttachmentPath = $"/Uploads/Attachments/{fileName}";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Dosya yükleme hatası: {ex.Message}");
                    response.AttachmentPath = null;
                }
            }

            // Update ticket status to "İşlemde" if it was "Açık"
            if (ticket.Status == "Açık")
            {
                ticket.Status = "İşlemde";
            }

            _context.TicketResponses.Add(response);
            await _context.SaveChangesAsync();

            // Send notification about the new response
            var responseResult = _mapper.Map<ResultTicketResponseDto>(response);

            // If staff responded, notify the ticket creator
            if (dto.IsStaff)
            {
                await _notificationService.SendTicketResponseNotificationAsync(
                    responseResult,
                    ticket.Subject,
                    ticket.Id,
                    ticket.UserId);
            }
            // If user responded, notify staff (send to all)
            else
            {
                await _notificationService.SendTicketResponseNotificationAsync(
                    responseResult,
                    ticket.Subject,
                    ticket.Id,
                    ""); // Empty userId means notification will be sent to all
            }
        }
        public async Task<bool> UpdateStatusAsync(Guid id, string status)
        {
            var ticket = await _context.SupportTickets.FindAsync(id);
            if (ticket == null)
                return false;

            ticket.Status = status;
            await _context.SaveChangesAsync();

            // Send notification about status change
            await _notificationService.SendTicketStatusChangedNotificationAsync(
                ticket.Subject,
                ticket.Id,
                status,
                ticket.UserId);

            return true;
        }

        public async Task<bool> CloseTicketAsync(Guid id)
        {
            var ticket = await _context.SupportTickets.FindAsync(id);
            if (ticket == null)
                return false;

            ticket.Status = "Kapatıldı";
            await _context.SaveChangesAsync();

            // Send notification about ticket being closed
            await _notificationService.SendTicketStatusChangedNotificationAsync(
                ticket.Subject,
                ticket.Id,
                "Kapatıldı",
                ticket.UserId);

            return true;
        }
    }
}
