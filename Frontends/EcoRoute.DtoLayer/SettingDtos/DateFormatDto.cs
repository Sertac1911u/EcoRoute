using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.SettingDtos
{
    public class DateFormatDto
    {
        public Guid Id { get; set; }
        public string FormatString { get; set; }
        public string Sample { get; set; }
    }
}
