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

namespace Mmcc.Stats.Features.TpsChartData
{
    public class Get
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
            public IList<ServerTpsChartData> ServersChartData;
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

                var data = (await _context.TpsStats.AsNoTracking()
                        .Include(s => s.Server).AsNoTracking()
                        .Where(x => x.StatTime.Date >= request.FromDateTime.Date &&
                                    x.StatTime.Date <= request.ToDateTime.Date)
                        .Select(s => new
                        {
                            s.ServerId,
                            s.Server.ServerName,
                            s.StatTime,
                            s.Tps
                        })
                        .ToListAsync(cancellationToken))
                    .GroupBy(queryResult => queryResult.ServerId)
                    .Select(serverTpsStat => new ServerTpsChartData
                    {
                        ServerName = serverTpsStat.First().ServerName,
                        Times = serverTpsStat.Select(x => x.StatTime),
                        Tps = serverTpsStat.Select(x => x.Tps)
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