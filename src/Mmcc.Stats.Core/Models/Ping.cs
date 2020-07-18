using System;

namespace Mmcc.Stats.Core.Models
{
    public class Ping
    {
        public int ServerId { get; set; }
        public DateTime PingTime { get; set; }
        public int PlayersOnline { get; set; }
        public int PlayersMax { get; set; }
    }
}