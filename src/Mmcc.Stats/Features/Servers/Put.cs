using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mmcc.Stats.Core.Data;

namespace Mmcc.Stats.Features.Servers
{
    public class Put
    {
        public class Command : IRequest<IActionResult>
        {
            public int ServerId { get; set; }
            public string ServerIp { get; set; }
            public int ServerPort { get; set; }
            public string ServerName { get; set; }
            public bool Enabled { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ServerId)
                    .NotNull()
                    .GreaterThan(0);
                
                RuleFor(x => x.ServerIp)
                    .NotNull()
                    .NotEmpty()
                    .Matches(@"\w+(\.\w+)?\.\w+
|^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");

                RuleFor(x => x.ServerPort)
                    .NotNull()
                    .GreaterThan(0);

                RuleFor(x => x.ServerName)
                    .NotNull()
                    .NotEmpty();

                RuleFor(x => x.Enabled)
                    .NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, IActionResult>
        {
            private readonly PollerContext _context;
            private readonly IMediator _mediator;

            public Handler(PollerContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            public async Task<IActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Entry(request).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    // check if exists and if doesn't return NotFound;
                    var res = await _mediator
                        .Send(new GetById.Query {Id = request.ServerId}, cancellationToken);
                    if (res is null)
                        return new NotFoundResult();

                    throw;
                }

                return new NoContentResult();
            }
        }
    }
}