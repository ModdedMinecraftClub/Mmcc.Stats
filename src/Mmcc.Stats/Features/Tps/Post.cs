using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Mmcc.Stats.Core.Data;
using Mmcc.Stats.Core.Data.Models;
using Mmcc.Stats.Core.Data.Models.Settings;
using Mmcc.Stats.Infrastructure.Commands;

namespace Mmcc.Stats.Features.Tps
{
    public class Post
    {
        public class Command : IRequest<TpsStat>
        {
            public int ServerId { get; set; }
            public double Tps { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.ServerId)
                    .NotNull()
                    .GreaterThanOrEqualTo(0);
                
                RuleFor(c => c.Tps)
                    .NotNull()
                    .GreaterThanOrEqualTo(0);
            }
        }

        public class Handler : IRequestHandler<Command, TpsStat>
        {
            private readonly PollerContext _context;
            private readonly IMediator _mediator;
            private readonly TpsPingSettings _settings;

            public Handler(PollerContext context, IMediator mediator, TpsPingSettings settings)
            {
                _context = context;
                _mediator = mediator;
                _settings = settings;
            }

            public async Task<TpsStat> Handle(Command request, CancellationToken cancellationToken)
            {
                // convert to TpsStat;
                var tpsStat = new TpsStat
                {
                    ServerId = request.ServerId,
                    Tps = request.Tps,
                    StatTime = DateTime.UtcNow
                };
                
                // notify staff if tps below set value;
                if (tpsStat.Tps < _settings.TpsToAlertAt)
                {
                    await _mediator.Send(new NotifyStaffAboutTps.Command {TpsStat = tpsStat},
                        cancellationToken);
                }
                
                // insert into db and return inserted entity;
                await _context.TpsStats.AddAsync(tpsStat, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return tpsStat;
            }
        }
    }
}