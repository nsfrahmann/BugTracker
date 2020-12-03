using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class ChartData
    {
        public string Name { get; set; }
        public double Count { get; set; }
        public double Percentage { get; set; }
        public string Color { get; set; }
    }
}
