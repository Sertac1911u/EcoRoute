using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.SupportDtos
{
    public class ResultSupportTicketDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Category { get; set; }
        public DateTime CreateDate { get; set; }
        public string AttachmentPath { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<ResultTicketResponseDto> Responses { get; set; } = new List<ResultTicketResponseDto>();
    }
}
