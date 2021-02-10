using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mmcc.Stats.Core.Data;

namespace Mmcc.Stats.Features.Pings
{
    public class GetWeeklyAvgs
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public IList<Average> ThisWeek { get; set; }
            public IList<Average> LastWeek { get; set; }

            public class Average
            {
                public string ServerName { get; set; }
                public double Avg { get; set; }
            }
        }
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly PollerContext _context;

            public Handler(PollerContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var thisWeekAvg = await _context.Pings
                    .AsNoTracking()
                    .Where(p => p.PingTime <= DateTime.UtcNow && p.PingTime >= DateTime.UtcNow.AddDays(-7))
                    .GroupBy(p => p.Server.ServerName)
                    .OrderBy(p => p.Key)
                    .Select(grouping => new Result.Average
                    {
                        ServerName = grouping.Key,
                        Avg = grouping.Average(p => p.PlayersOnline)
                    })
                    .ToListAsync(cancellationToken);

                var lastWeekAvg = await _context.Pings
                    .AsNoTracking()
                    .Where(p => p.PingTime <= DateTime.UtcNow.AddDays(-7) &&
                                p.PingTime >= DateTime.UtcNow.AddDays(-14))
                    .GroupBy(p => p.Server.ServerName)
                    .OrderBy(p => p.Key)
                    .Select(grouping => new Result.Average
                    {
                        ServerName = grouping.Key,
                        Avg = grouping.Average(p => p.PlayersOnline)
                    })
                    .ToListAsync(cancellationToken);
                
                return new Result
                {
                    ThisWeek = thisWeekAvg,
                    LastWeek = lastWeekAvg
                };
            }
        }
    }
}