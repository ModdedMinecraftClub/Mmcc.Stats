using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mmcc.Stats.Core.Data;
using Mmcc.Stats.Core.Data.Models;

namespace Mmcc.Stats.Features.Tps
{
    public class Get
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public IList<TpsStat> TpsStats { get; set; }
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
                var data = await _context.TpsStats.AsNoTracking().ToListAsync(cancellationToken);
                return new Result
                {
                    TpsStats = data
                };
            }
        }
    }
}