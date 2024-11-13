using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;

public class TokenStore
{
    private static readonly Dictionary<int, string> _activeTokens = new Dictionary<int, string>();

    public static void UpdateActiveTokenForUser(int userId, string newToken)
    {
        _activeTokens[userId] = newToken;
    }

    public static bool IsValidTokenForUser(int userId, string token)
    {
        if (_activeTokens.TryGetValue(userId, out var activeToken))
        {
            return activeToken == token;
        }
        return false;
    }
    public static int? FindUserIdByToken(string token)
    {
        foreach (var entry in _activeTokens)
        {
            if (entry.Value == token)
            {
                return entry.Key; // Return the user ID
            }
        }

        return null; // Return null if no matching token is found
    }

    public static Task ValidateToken(TokenValidatedContext context)
    {
        var userIdClaim = context.Principal.FindFirst("id").Value;
        if (int.TryParse(userIdClaim, out var userId))
        {
            var token = context.SecurityToken as JwtSecurityToken;
            var tokenString = token?.RawData;

            // Assuming TokenStore is your custom class for managing tokens
            if (!IsValidTokenForUser(userId, tokenString))
            {
                // This will reject the token if it's not the current active token for the user
                context.Fail("Invalid token.");
            }
        }
        else
        {
            context.Fail("User ID claim is missing.");
        }

        return Task.CompletedTask;
    }
}
