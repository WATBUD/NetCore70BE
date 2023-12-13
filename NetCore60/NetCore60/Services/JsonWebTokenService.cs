using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
namespace NetCore60.Services
{

    public static class JsonWebTokenService
    {
        public static string baseSecretKey = "es+E1CdOQA5jRehCOM04qylj2blOd1edxYZmaCGt0co=";


        public static string GenerateSecretKey(int keyLengthInBytes)
        {
            byte[] keyBytes = new byte[keyLengthInBytes];
            RandomNumberGenerator.Fill(keyBytes);
            return Convert.ToBase64String(keyBytes);
        }
        //public static string GenerateJwtToken(string username, int expirationMinutes = 60)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.UTF8.GetBytes(baseSecretKey);

        //    var claims = new ClaimsIdentity(new[]
        //    {
        //    new Claim(ClaimTypes.Name, username)
        //});

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = claims,
        //        Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
        //        Issuer ="Net60Server",
        //        Audience = "RNDatingApp",
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}
        public static string GenerateJwtToken(int userId, int expirationMinutes = 120)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(baseSecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("id", userId.ToString())
                }),
                //        Issuer ="Net60Server",
                //        Audience = "RNDatingApp",
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            // Invalidate previous token and update the store with the new token
            TokenStore.UpdateActiveTokenForUser(userId, tokenString);

            return tokenString;
        }
        public static int TryGetUserIdFromJwtToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(baseSecretKey)),
                };

                SecurityToken validatedToken;
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                if (claimsPrincipal.Identity is ClaimsIdentity claimsIdentity)
                {
                    var idClaim = claimsIdentity.FindFirst("id");
                    if (idClaim != null && int.TryParse(idClaim.Value, out int userId))
                    {
                        return userId; // 验证成功，返回用户ID
                    }
                }

                return -1; // 未找到用户ID或解析失败，返回默认值表示验证失败
            }
            catch (SecurityTokenExpiredException)
            {
                // 令牌已过期，处理过期情况
                return -1; // 返回默认值表示验证失败
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                // 令牌签名无效，处理签名无效情况
                return -1; // 返回默认值表示验证失败
            }
            catch (Exception)
            {
                // 其他验证异常，处理其他验证失败情况
                return -1; // 返回默认值表示验证失败
            }
        }



    }

}




