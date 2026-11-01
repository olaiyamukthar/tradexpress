using MediatR;

namespace TradeXpress.Business.Auth;

public record RegisterCompanyCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string CompanyName,
    string RegistrationNumber,
    string TaxId,
    string RegisteredAddress
) : IRequest<Guid>;
