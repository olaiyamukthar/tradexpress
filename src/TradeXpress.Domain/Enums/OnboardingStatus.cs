namespace TradeXpress.Domain;

public enum OnboardingStage
{
    Registered,
    EmailVerified,
    KycSubmitted,
    KycApproved,
    KycRejected,
    Active
}