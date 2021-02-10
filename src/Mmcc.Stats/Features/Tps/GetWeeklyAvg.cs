using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mmcc.Stats.Core.Data;

namespace Mmcc.Stats.Features.Tps
{
    public class GetWeeklyAvg
    {
        public class Query : IRequest<Result>
        {
        }
        
        public class Result
        {
            public double ThisWeekAvg { get; set; }
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
                double avg;
                
                try
                {
                    avg = await _context.TpsStats
                        .AsNoTracking()
                        .Where(t => t.StatTime <= DateTime.UtcNow && t.StatTime >= DateTime.UtcNow.AddDays(-7))
                        .AverageAsync(t => t.Tps, cancellationToken);
                }
                catch (InvalidOperationException e)
                {
                    if (e.Message.Contains("Sequence contains no elements."))
                    {
                        avg = 0;
                    }
                    else
                    {
                        throw;
                    }
                }

                return new Result
                {
                    ThisWeekAvg = avg
                };
            }
        }
    }
}