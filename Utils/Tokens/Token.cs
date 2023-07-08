using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Utils.Tokens;

public static class Token
{
    public const string SecretKey = "12341234123412341234123412341234";
    static Token()
    {
    }

    public static string GenerateToken(string issuer, string audience)
    {
        var claims = new Claim[]
        {
            
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddHours(12),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)), 
                SecurityAlgorithms.HmacSha256)
            );
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public static bool ValidateToken(string token, string issuer, string audience)
    {
        if (token is null)
        {
            return false;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)), // 使用与生成令牌时相同的密钥
            ValidateIssuer = true,
            ValidIssuer = issuer, // 验证令牌发行人
            ValidateAudience = true,
            ValidAudience = audience, // 验证令牌受众
            ValidateLifetime = true, // 验证令牌是否过期
            ClockSkew = TimeSpan.Zero // 设置时间偏移量为零，即不允许任何偏移量
        };
        

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return true; // 令牌有效
        }
        catch (Exception)
        {
            return false; // 令牌无效
        }
    }
}
