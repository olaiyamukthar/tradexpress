using MediatR;

namespace TradeXpress.Business.Auth;

public record LoginResult(string AccessToken, DateTime Expiration);
