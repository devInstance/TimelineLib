using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInstance.TimelineLib.Model
{
    struct TimeBar
    {
        public string CssClass { get; set; }
        public string Left { get; set; }
        public string Top { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Label { get; set; }
        public string LabelX { get; set; }
        public string LabelY { get; set; }
        public string Tooltip { get; set; }
    }
}
