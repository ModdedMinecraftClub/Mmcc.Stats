using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mmcc.Stats.Core.Data;
using Mmcc.Stats.Core.Data.Models;

namespace Mmcc.Stats.Features.Tps
{
    public class Get
    {
        public class Query : IRequest<TpsStat>
        {
            public int Id { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Id)
                    .NotNull()
                    .GreaterThanOrEqualTo(0);
            }
        }

        public class Handler : IRequestHandler<Query, TpsStat>
        {
            private readonly PollerContext _context;

            public Handler(PollerContext context)
            {
                _context = context;
            }

            public async Task<TpsStat> Handle(Query request, CancellationToken cancellationToken)
            {
                var res = await _context.TpsStats.AsNoTracking()
                    .Where(t => t.Id == request.Id)
                    .SingleOrDefaultAsync(cancellationToken);
                return res;
            }
        }
    }
}