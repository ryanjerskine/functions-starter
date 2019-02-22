using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionsStarter.Features.SetIndividualInfo
{
    public class Command : IRequest
    {
        public string Name { get; }
        public string Email { get; }

        public Command(string name, string email)
        {
            this.Name = name;
            this.Email = email;
        }
    }
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Name is required.");
            RuleFor(x => x.Name)
                .MinimumLength(5)
                .WithMessage("Name must be at least 5 characters.");
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("Email is required.");
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Invalid email address.");
        }
    }
    public class CommandHandler : IRequestHandler<Command>
    {
        private readonly Junk.DatabaseContext _DatabaseContext;
        private readonly Junk.NameFormatter _NameFormatter;

        public CommandHandler(Junk.DatabaseContext databaseContext, Junk.NameFormatter nameFormatter)
        {
            this._DatabaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            this._NameFormatter = nameFormatter ?? throw new ArgumentNullException(nameof(nameFormatter));
        }

        public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            var formattedName = this._NameFormatter.Format(command.Name);

            await this._DatabaseContext.Individuals.AddAsync(new Junk.Individual()
            {
                FirstName = formattedName.First,
                LastName = formattedName.Last,
                Email = command.Email
            });

            return Unit.Value;
        }
    }
}