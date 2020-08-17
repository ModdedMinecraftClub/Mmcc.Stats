using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mmcc.Stats.Core.Data;
using Mmcc.Stats.Core.Data.Models;

namespace Mmcc.Stats.Features.Pings
{
    public class GetByServerId
    {
        public class Query : IRequest<Result>
        {
            public int ServerId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.ServerId)
                    .NotNull()
                    .GreaterThan(0);
            }
        }

        public class Result
        {
            public IList<Ping> Pings { get; set; }
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
                var data = await _context.Pings
                    .AsNoTracking()
                    .Where(x => x.ServerId == request.ServerId)
                    .ToListAsync(cancellationToken);
                return new Result
                {
                    Pings = data
                };
            }
        }
    }
}