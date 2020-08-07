using System;

namespace Mmcc.Stats.Core.Models.Dto
{
    public class ServerPingDto
    {
        public int ServerId { get; set; }
        public string ServerName { get; set; }
        public DateTime PingTime { get; set; }
        public int PlayersOnline { get; set; }
    }
}