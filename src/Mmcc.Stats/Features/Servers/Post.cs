using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Mmcc.Stats.Core.Data;
using Mmcc.Stats.Core.Data.Models;

namespace Mmcc.Stats.Features.Servers
{
    public class Post
    {
        public class Command : IRequest<Server>
        {
            public string ServerIp { get; set; }
            public int ServerPort { get; set; }
            public string ServerName { get; set; }
            public bool Enabled { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
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

        public class Handler : IRequestHandler<Command, Server>
        {
            private readonly PollerContext _context;

            public Handler(PollerContext context)
            {
                _context = context;
            }

            public async Task<Server> Handle(Command request, CancellationToken cancellationToken)
            {
                // convert to server;
                var server = new Server
                {
                    ServerIp = request.ServerIp,
                    ServerName = request.ServerName,
                    ServerPort = request.ServerPort,
                    Enabled = request.Enabled
                };
                
                await _context.Servers.AddAsync(server, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return server;
            }
        }
    }
}