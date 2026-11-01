namespace TradeXpress.Domain.Entities;

public class CompanyProfile
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public string CompanyName { get; private set; }
    public string RegistrationNumber { get; private set; }
    public string TaxId { get; private set; }
    public string RegisteredAddress { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public CompanyProfile(Guid accountId, string companyName, string registrationNumber, string taxId, string registeredAddress)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        CompanyName = companyName;
        RegistrationNumber = registrationNumber;
        TaxId = taxId;
        RegisteredAddress = registeredAddress;
        CreatedAt = DateTime.UtcNow;
    }
}
