using System;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Dto;

namespace Mmcc.Stats.Infrastructure.Extensions
{
    public static class DtoConvertExtensions
    {
        public static TpsStat AsTpsStat(this McTpsStatDto dto)
            => new TpsStat
            {
                ServerId = dto.ServerId,
                StatTime = DateTime.UtcNow,
                Tps = dto.Tps
            };
    }
}