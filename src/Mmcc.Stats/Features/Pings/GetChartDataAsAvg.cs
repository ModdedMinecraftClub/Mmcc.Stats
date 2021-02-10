using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MathNet.Numerics.Statistics;
using MediatR;
using Mmcc.Stats.Core.Data.Dtos;

namespace Mmcc.Stats.Features.Pings
{
    public class GetChartDataAsAvg
    {
        public class Query : IRequest<Result>
        {
            public DateTime FromDateTime { get; set; }
            public DateTime ToDateTime { get; set; }
            public int WindowSize { get; set; }
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

                RuleFor(x => x.WindowSize)
                    .NotNull()
                    .GreaterThan(0);
            }
        }

        public class Result
        {
            public IList<ServerPlayerbaseChartData> ServerAvgChartDataDtos { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IMediator _mediator;

            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = new GetChartData.Query{ FromDateTime = request.FromDateTime, ToDateTime = request.ToDateTime};
                var chartData = (await _mediator.Send(query, cancellationToken)).ServersChartData;
                var avgData = chartData.Select(server => new ServerPlayerbaseChartData
                {
                    ServerName = server.ServerName, 
                    Players = server.Players.MovingAverage(request.WindowSize),
                    Times = server.Times
                }).ToList();
                return new Result
                {
                    ServerAvgChartDataDtos = avgData
                };
            }
        }
    }
}