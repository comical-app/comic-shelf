using Models.Responses;
using Models.ServicesInterfaces;

namespace Services;

public class JwtTokenService : IJwtTokenService
{
    public Task<string> GenerateToken()
    {
        throw new NotImplementedException();
    }

    public Task<string> GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }

    public Task<Token> ValidateToken(string token)
    {
        throw new NotImplementedException();
    }

    public Task<Token> ValidateRefreshToken(string refreshToken)
    {
        throw new NotImplementedException();
    }
}