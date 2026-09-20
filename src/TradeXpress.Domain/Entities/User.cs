using TradeXpress.Domain.Enums;
namespace TradeXpress.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PasswordHash { get; private set; }
    public OnboardingStage OnboardingStatus { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User(string email, string firstName, string lastName, string passwordHash)
    {
        Id = Guid.NewGuid();
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = passwordHash;
        OnboardingStatus = OnboardingStage.Registered;
        CreatedAt = DateTime.UtcNow;
    }
}
