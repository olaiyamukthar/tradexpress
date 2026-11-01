using MediatR;
using Microsoft.AspNetCore.Identity;
using TradeXpress.Business.Common;
using TradeXpress.Domain.Entities;
using TradeXpress.Domain.Enums;

namespace TradeXpress.Business.Auth;

public class RegisterCompanyCommandHandler : IRequestHandler<RegisterCompanyCommand, Guid>
{
    private readonly IApplicationDbContext _dbContext;

    public RegisterCompanyCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(
        RegisterCompanyCommand request,
        CancellationToken cancellationToken
    )
    {
        var hasher = new PasswordHasher<User>();
        var passwordHash = hasher.HashPassword(null!, request.Password);

        var user = new User(request.Email, request.FirstName, request.LastName, passwordHash);
        var account = new Account(AccountState.Company);
        var accountUser = new AccountUser(user.Id, account.Id, AccountUserRole.Owner);
        var companyProfile = new CompanyProfile(
            account.Id,
            request.CompanyName.Trim(),
            request.RegistrationNumber.Trim(),
            request.TaxId.Trim(),
            request.RegisteredAddress.Trim()
        );

        _dbContext.Users.Add(user);
        _dbContext.Accounts.Add(account);
        _dbContext.AccountUsers.Add(accountUser);
        _dbContext.CompanyProfiles.Add(companyProfile);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
