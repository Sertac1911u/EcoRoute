﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.SupportDtos
{
    public class UpdateStatusDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = null!;
    }
}
