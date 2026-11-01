using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TradeXpress.Business.Common;

namespace TradeXpress.Business.Auth;

public class RegisterCompanyCommandValidator : AbstractValidator<RegisterCompanyCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public RegisterCompanyCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(
                async (email, cancellationToken) =>
                {
                    return !await _dbContext.Users.AnyAsync(
                        u => u.Email == email,
                        cancellationToken
                    );
                }
            )
            .WithMessage("This email is already registered.");

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]")
            .WithMessage("Password must contain at least one number.")
            .Matches(@"[^a-zA-Z0-9\s]")
            .WithMessage("Password must contain at least one special character.");

        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");

        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Company name must not exceed 200 characters.");

        RuleFor(x => x.RegistrationNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Registration number must not exceed 50 characters.")
            .MustAsync(
                async (registrationNumber, cancellationToken) =>
                {
                    return !await _dbContext.CompanyProfiles.AnyAsync(
                        cp => cp.RegistrationNumber == registrationNumber.Trim(),
                        cancellationToken
                    );
                }
            )
            .WithMessage("This registration number is already in use.");

        RuleFor(x => x.TaxId)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Tax ID must not exceed 50 characters.");

        RuleFor(x => x.RegisteredAddress)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Registered address must not exceed 500 characters.");
    }
}
