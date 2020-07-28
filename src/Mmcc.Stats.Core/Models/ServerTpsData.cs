using System.Collections.Generic;
using Mmcc.Stats.Core.Models.Dto;

namespace Mmcc.Stats.Core.Models
{
    public class ServerTpsData
    {
        public string ServerName { get; set; }
        public IEnumerable<ServerTpsDto> TpsStats { get; set; }
    }
}