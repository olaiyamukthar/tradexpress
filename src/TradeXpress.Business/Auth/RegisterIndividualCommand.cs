using MediatR;
namespace TradeXpress.Business.Auth;

public record RegisterIndividualCommand(string Email, string FirstName, string LastName, string Password) : IRequest<Guid>;