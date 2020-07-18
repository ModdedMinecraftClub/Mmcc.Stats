using System;
using System.Collections.Generic;

namespace Mmcc.Stats.Core.Models
{
    public class ServerPlayerbaseData
    {
        public string ServerName { get; set; }
        public IList<DateTime> TimesList { get; set; }
        public IList<int> PlayersOnlineList { get; set; }
    }
}