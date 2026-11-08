using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradeXpress.Business.Common;
using TradeXpress.Domain.Entities;

namespace TradeXpress.Business.Auth;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IJwtTokenService _tokenService;
    private readonly PasswordHasher<User> _passwordHasher;

    // Generate the dummy hash once at startup to shield the app from timing attacks
    private static readonly string DummyHash = new PasswordHasher<User>().HashPassword(
        null!,
        "not-a-real-password"
    );

    public LoginCommandHandler(IApplicationDbContext dbContext, IJwtTokenService tokenService)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
        {
            _passwordHasher.VerifyHashedPassword(null!, DummyHash, request.Password);
            throw new AuthenticationFailedException();
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new AuthenticationFailedException();
        }

        return _tokenService.GenerateToken(user);
    }
}
