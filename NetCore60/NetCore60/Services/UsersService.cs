using NetCore60.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetCore60.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static NetCore60.DTO.ResponseDTO;
public class UsersService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IConfiguration _configuration;
    public UsersService(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
        _configuration = configuration;

    }
    public async Task<ResponseDTO> Login(LoginDTO model)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserAccount == model.UserAccount);

        if (user == null)
        {
            return ErrorResponse("Error", new { message = "User does not exist." });

        }

        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return ErrorResponse("Error", new { message = "Incorrect password." });
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return SuccessResponse(new { Token = tokenHandler.WriteToken(token) });
    }


}