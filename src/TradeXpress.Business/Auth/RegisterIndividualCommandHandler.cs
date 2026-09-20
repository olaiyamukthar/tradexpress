using MediatR;
using Microsoft.AspNetCore.Identity;
using TradeXpress.Domain.Entities;
using TradeXpress.Domain.Enums;
using TradeXpress.Business.Common;

namespace TradeXpress.Business.Auth;

public class RegisterIndividualCommandHandler : IRequestHandler<RegisterIndividualCommand, Guid>
{
    private readonly IApplicationDbContext _dbContext;

    public RegisterIndividualCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(RegisterIndividualCommand request, CancellationToken cancellationToken)
    {
        var hasher = new PasswordHasher<User>();
        var passwordHash = hasher.HashPassword(null!, request.Password);

        var user = new User(request.Email, request.FirstName, request.LastName, passwordHash);
        var account = new Account(AccountState.Individual);
        var accountUser = new AccountUser(user.Id, account.Id, AccountUserRole.Owner);

        _dbContext.Users.Add(user);
        _dbContext.Accounts.Add(account);
        _dbContext.AccountUsers.Add(accountUser);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}