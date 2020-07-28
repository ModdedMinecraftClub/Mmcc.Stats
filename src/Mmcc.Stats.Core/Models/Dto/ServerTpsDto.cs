using System;

namespace Mmcc.Stats.Core.Models.Dto
{
    public class ServerTpsDto
    {
        public DateTime Time { get; set; }
        public double Tps { get; set; }
    }
}