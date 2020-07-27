using System;
using System.Text.Json.Serialization;
using Mmcc.Stats.Core.Json;

namespace Mmcc.Stats.Core.Models
{
    public class TpsStat
    {
        public int ServerId { get; set; }
        [JsonConverter(typeof(JavaDateConverter))]
        public DateTime StatTime { get; set; }
        public double Tps { get; set; }
    }
}