using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.SettingDtos
{
    public class ThemeColorDto
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Dictionary<string, string> Shades { get; set; } = new Dictionary<string, string>();

    }
}
