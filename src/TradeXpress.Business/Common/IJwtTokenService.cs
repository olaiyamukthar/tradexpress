using TradeXpress.Business.Auth;
using TradeXpress.Domain.Entities;

namespace TradeXpress.Business.Common;

public interface IJwtTokenService
{
    LoginResult GenerateToken(User user);
}
