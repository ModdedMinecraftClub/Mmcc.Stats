using System;
using System.Collections.Generic;

namespace Mmcc.Stats.Core.Data.Dtos
{
    public class ServerTpsChartData
    {
        public string ServerName { get; set; }
        public IEnumerable<DateTime> Times { get; set; }
        public IEnumerable<double> Tps { get; set; }
    }
}