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
    public class GetById
    {
        public class Query : IRequest<Ping>
        {
            public int Id { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Id)
                    .NotNull()
                    .GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Query, Ping>
        {
            private readonly PollerContext _context;

            public Handler(PollerContext context)
            {
                _context = context;
            }

            public async Task<Ping> Handle(Query request, CancellationToken cancellationToken)
            {
                var res = await _context.Pings
                    .AsNoTracking()
                    .Where(x => x.Id == request.Id)
                    .FirstOrDefaultAsync(cancellationToken);
                return res;
            }
        }
    }
}