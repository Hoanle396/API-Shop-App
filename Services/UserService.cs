namespace WebAPI.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Entities;
using WebAPI.Helpers;
using WebAPI.Models;
using BCrypt.Net;
using WebAPI.Models.Request;

public interface IUserService {
    Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
    Task<IEnumerable<User?>> GetAll();
    User? GetById(int id);
    Task<User?> Register(RegisterRequest model);
}

public class UserService : IUserService {
    private DBContext ctx = new DBContext();

    public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model) {
        try {
            var user = await ctx.users.Where(x => x.Email == model.Email).FirstOrDefaultAsync();

            if (user != null && BCrypt.Verify(model.Password, user.Password)) {
                var token = generateJwtToken(user);

                return new AuthenticateResponse(user, token);
            }

            return null;
        } catch {
            throw;
        }
    }

    public async Task<IEnumerable<User?>> GetAll() {
        try {
            return await ctx.users.ToListAsync();
        } catch {
            throw;
        }
    }

    public User? GetById(int id) {
        try {
            return ctx.users.Where(x => x.Id == id).FirstOrDefault();
        } catch(Exception e) {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task<User?> Register(RegisterRequest model) {
        try {
            if (ctx.users.Any(x => x.Email == model.Email))
                throw new ApplicationException("Email '" + model.Email + "' is already taken");

            var user = new User {
                Email = model.Email,
                FullName = model.Fullname,
            };

            user.Password = BCrypt.HashPassword(model.Password);

            await ctx.users.AddAsync(user);
            await ctx.SaveChangesAsync();
            return user;
        } catch {
            throw;
        }
    }

    private string generateJwtToken(User user) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}