﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VmstatsGUI
{
    public class ProcessCommand
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string VmPattern { get; set; }
        public string Dsl { get; set; }
        public string ConnectionId { get; set; }
    }
}
