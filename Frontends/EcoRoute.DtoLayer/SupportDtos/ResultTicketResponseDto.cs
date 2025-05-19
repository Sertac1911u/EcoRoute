using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.SupportDtos
{
    public class ResultTicketResponseDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime ResponseDate { get; set; }
        public bool IsStaff { get; set; }
        public string AttachmentPath { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
