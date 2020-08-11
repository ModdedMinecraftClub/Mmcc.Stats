using System;
using System.Collections.Generic;

namespace Mmcc.Stats.Core.Data.Dtos
{
    public class ServerChartDataDto
    {
        public string ServerName { get; set; }
        public IEnumerable<DateTime> Times { get; set; }
        public IEnumerable<double> Players { get; set; }
    }
}