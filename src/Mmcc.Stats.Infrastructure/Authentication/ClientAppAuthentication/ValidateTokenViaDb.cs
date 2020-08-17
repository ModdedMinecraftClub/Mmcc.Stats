using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mmcc.Stats.Core.Data;
using Mmcc.Stats.Core.Data.Models;

namespace Mmcc.Stats.Infrastructure.Authentication.ClientAppAuthentication
{
    public class ValidateTokenViaDb
    {
        public class Query : IRequest<Result>
        {
            public string IncomingTokenValue { get; set; }
        }

        public class Result
        {
            public ClientToken ValidatedToken { get; set; }
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
                var validatedToken = await _context.ClientTokens.AsNoTracking()
                    .Where(x => x.Value.Equals(request.IncomingTokenValue))
                    .SingleOrDefaultAsync(cancellationToken);
                return new Result
                {
                    ValidatedToken = validatedToken
                };
            }
        }
    }
}