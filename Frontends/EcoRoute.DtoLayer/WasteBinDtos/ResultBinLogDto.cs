using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.WasteBinDtos
{
    public class ResultBinLogDto
    {
        public Guid BinLogId { get; set; }
        public Guid WasteBinId { get; set; }
        public double? FillLevel { get; set; }
        public string DeviceStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
