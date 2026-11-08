namespace TradeXpress.Business.Common;

public class AuthenticationFailedException : Exception
{
    public AuthenticationFailedException()
        : base("Invalid email or password.") { }

    public AuthenticationFailedException(string message)
        : base(message) { }
}
