using Models.Responses;

namespace Models.ServicesInterfaces;

public interface IJwtTokenService
{
    Task<string> GenerateToken();
    
    Task<string> GenerateRefreshToken();
    
    Task<Token> ValidateToken(string token);
    
    Task<Token> ValidateRefreshToken(string refreshToken);
}