namespace TradeXpress.Domain;

public class AccountUser
{
    public Guid UserId { get; private set; }
    public Guid AccountId { get; private set; }
    public AccountUserRole Role { get; private set; }

    public AccountUser(Guid userId, Guid accountId, AccountUserRole role)
    {
        UserId = userId;
        AccountId = accountId;
        Role = role;
    }
}
