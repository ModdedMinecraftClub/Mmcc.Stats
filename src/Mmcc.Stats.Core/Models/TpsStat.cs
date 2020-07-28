using System;

namespace Mmcc.Stats.Core.Models
{
    public class TpsStat
    {
        public int ServerId { get; set; }
        public DateTime StatTime { get; set; }
        public double Tps { get; set; }
    }
}