namespace TradeXpress.Domain;

public class Account
{
    public Guid Id { get; private set; }
    public AccountState AccountType { get; private set; }
    public ComplianceStage ComplianceStatus { get; private set; }
    public int KycTier { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Account(AccountState accountType)
    {
        Id = Guid.NewGuid();
        KycTier = 0;
        AccountType = accountType;
        ComplianceStatus = ComplianceStage.Unverified;
        CreatedAt = DateTime.UtcNow;
    }
}
