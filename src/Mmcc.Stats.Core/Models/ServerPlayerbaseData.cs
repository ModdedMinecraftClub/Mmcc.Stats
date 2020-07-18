using System.Collections.Generic;

namespace Mmcc.Stats.Core.Models
{
    public class ServerPlayerbaseData
    {
        public string ServerName { get; set; }
        public List<ServerPlayerbaseDataPoint> Data { get; set; }
    }
}