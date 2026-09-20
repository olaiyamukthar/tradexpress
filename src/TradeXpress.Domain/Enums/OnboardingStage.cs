namespace TradeXpress.Domain.Enums;

public enum OnboardingStage
{
    Registered,
    EmailVerified,
    KycSubmitted,
    KycApproved,
    KycRejected,
    Active
}