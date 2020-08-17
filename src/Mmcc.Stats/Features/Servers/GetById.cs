using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mmcc.Stats.Core.Data;
using Mmcc.Stats.Core.Data.Dtos;
using Mmcc.Stats.Infrastructure.Extensions;

namespace Mmcc.Stats.Features.Servers
{
    public class GetById
    {
        public class Query : IRequest<ServerDto>
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

        public class Handler : IRequestHandler<Query, ServerDto>
        {
            private readonly PollerContext _context;

            public Handler(PollerContext context)
            {
                _context = context;
            }

            public async Task<ServerDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var res = await _context.Servers
                    .AsNoTracking()
                    .Where(x => x.ServerId == request.Id)
                    .Select(s => s.ToServerDto())
                    .FirstOrDefaultAsync(cancellationToken);
                return res;
            }
        }
    }
}