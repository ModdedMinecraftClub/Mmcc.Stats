using System;
using System.Collections.Generic;
using Mmcc.Stats.Core.Models.Dto;

namespace Mmcc.Stats.Core.Models
{
    public class ServerPlayerbaseData
    {
        public string ServerName { get; set; }
        public IEnumerable<DateTime> Times { get; set; }
        public IEnumerable<double> Players { get; set; }
    }
}