using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.NotificationDtos
{
    public class UpdateNotificationStatusDto
    {
        public Guid Id { get; set; }
        public bool IsRead { get; set; }

    }
}
