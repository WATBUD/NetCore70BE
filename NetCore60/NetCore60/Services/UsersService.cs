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



}