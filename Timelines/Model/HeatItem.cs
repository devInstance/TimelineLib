﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInstance.Timelines.Model
{
    struct HeatItem
    {
        public string CssClass { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public float Value { get; set; }
        public string Tooltip { get; set; }
    }
}
