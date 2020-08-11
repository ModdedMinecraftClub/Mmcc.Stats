using System;

namespace Mmcc.Stats.Core.Data.Models
{
    public class Ping
    {
        public int Id { get; set; }
        public int ServerId { get; set; }
        public DateTime PingTime { get; set; }
        public int PlayersOnline { get; set; }
        public int PlayersMax { get; set; }

        public virtual Server Server { get; set; }
    }
}
