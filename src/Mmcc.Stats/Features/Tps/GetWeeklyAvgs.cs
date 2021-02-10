using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mmcc.Stats.Core.Data;
using Mmcc.Stats.Core.Data.Dtos;

namespace Mmcc.Stats.Features.Tps
{
    public class GetWeeklyAvgs
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public IList<Average> Averages { get; set; }
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
                var avg = await _context.TpsStats
                    .AsNoTracking()
                    .Where(t => t.StatTime <= DateTime.UtcNow && t.StatTime >= DateTime.UtcNow.AddDays(-7))
                    .GroupBy(t => t.Server.ServerName)
                    .OrderBy(g => g.Key)
                    .Select(g => new Result.Average
                    {
                        ServerName = g.Key,
                        Avg = g.Average(ts => ts.Tps)
                    })
                    .ToListAsync(cancellationToken);
                
                return new Result
                {
                    Averages = avg
                };
            }
        }
    }
}