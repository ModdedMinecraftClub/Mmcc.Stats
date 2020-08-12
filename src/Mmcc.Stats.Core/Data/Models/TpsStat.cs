using System;

namespace Mmcc.Stats.Core.Data.Models
{
    public class TpsStat
    {
        public int Id { get; set; }
        public int ServerId { get; set; }
        public DateTime StatTime { get; set; }
        public double Tps { get; set; }

        public virtual Server Server { get; set; }
    }
}
