using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.Application.Exceptions;
using Identity.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Application.Services;

public class UsersService
{
    private readonly IUsersRepository _repository;
    private readonly IPasswordHasher _hasher;
    private readonly IConfiguration _configuration;

    public UsersService(IUsersRepository repository, IPasswordHasher hasher, IConfiguration configuration)
    {
        _repository = repository;
        _hasher = hasher;
        _configuration = configuration;
    }
    
    public async Task<string?> Login(string email, string password)
    {
        string pepper = _configuration["Auth:Pepper"] ?? string.Empty;
        var user = await _repository.FindByEmailAsync(email);
        
        if (user == null || !_hasher.Verify(password + pepper, user.PasswordHash)) return null;

        if (_configuration["Auth:Key"] == null || _configuration["Auth:Expires"] == null)
        {
            throw new ConfigurationMissingException("Auth config is missed");
        }

        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:Key"]!)),
                SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            signingCredentials: signingCredentials,
            claims: new List<Claim>() { new Claim("userId", user.Id.ToString()) },
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Auth:Expires"]))
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    
}