using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mmcc.Stats.Core.Data;
using Mmcc.Stats.Core.Data.Dtos;

namespace Mmcc.Stats.Features.Pings
{
    public class GetChartData
    {
        public class Query : IRequest<Result>
        {
            public DateTime FromDateTime { get; set; }
            public DateTime ToDateTime { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.FromDateTime)
                    .NotNull()
                    .LessThanOrEqualTo(x => x.ToDateTime);

                RuleFor(x => x.ToDateTime)
                    .NotNull();
            }
        }

        public class Result
        {
            public IList<ServerPlayerbaseChartData> ServersChartData { get; set; }
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
                if (request.FromDateTime == request.ToDateTime)
                {
                    request.ToDateTime = request.ToDateTime.AddDays(1);
                }

                var data = (await _context.Pings
                        .AsNoTracking()
                        .Include(s => s.Server)
                        .Where(x => x.PingTime.Date >= request.FromDateTime.Date &&
                                    x.PingTime.Date <= request.ToDateTime.Date)
                        .Select(s => new
                        {
                            s.ServerId,
                            s.Server.ServerName,
                            s.PingTime,
                            s.PlayersOnline
                        })
                        .ToListAsync(cancellationToken))
                    .GroupBy(ping => ping.ServerId)
                    .Select(serverPing => new ServerPlayerbaseChartData
                    {
                        ServerName = serverPing.First().ServerName,
                        Times = serverPing.Select(x => x.PingTime),
                        Players = serverPing.Select(x => (double) x.PlayersOnline)
                    })
                    .ToList();

                return new Result
                {
                    ServersChartData = data
                };
            }
        }
    }
}