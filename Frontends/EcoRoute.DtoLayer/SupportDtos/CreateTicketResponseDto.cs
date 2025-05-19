

using Microsoft.AspNetCore.Http;

namespace EcoRoute.DtoLayer.SupportDtos
{
    public class CreateTicketResponseDto
    {
        public Guid SupportTicketId { get; set; }
        public string Message { get; set; }
        public bool IsStaff { get; set; }
        public IFormFile Attachment { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
