using MediatR;

namespace TradeXpress.Business.Auth;

public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;
