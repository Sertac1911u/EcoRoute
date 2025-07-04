﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.WasteBinDtos
{
    public class UpdateSensorDto
    {
        public Guid SensorId { get; set; }
        public bool IsActive { get; set; }
        public bool IsWorking { get; set; }
        public DateTime? LastUpdate { get; set; } = DateTime.Now;
    }
}
