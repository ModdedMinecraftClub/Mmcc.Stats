using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mmcc.Stats.Core.Data;
using Mmcc.Stats.Core.Data.Dtos;
using Mmcc.Stats.Infrastructure.Extensions;

namespace Mmcc.Stats.Features.Servers
{
    public class Get
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public IList<ServerDto> Servers { get; set; }
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
                var data = await _context.Servers
                    .AsNoTracking()
                    .Select(s => s.ToServerDto())
                    .ToListAsync(cancellationToken);
                return new Result
                {
                    Servers = data
                };
            }
        }
    }
}