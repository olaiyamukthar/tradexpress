using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TradeXpress.Business.Common;

namespace TradeXpress.Business.Auth;

public class RegisterIndividualCommandValidator : AbstractValidator<RegisterIndividualCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public RegisterIndividualCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Email)
        .NotEmpty()
        .EmailAddress()
        .MustAsync(async (email, cancellationToken) =>
        {
            return !await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
        })
        .WithMessage("This email is already registered.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");

        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();
    }
}