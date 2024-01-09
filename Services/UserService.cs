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

public interface IUserService
{
  Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
  Task<IEnumerable<User?>> Find(FindRequest model);
  User? GetById(int id);
  Task<User?> Register(RegisterRequest model);
  Task<bool?> DeleteById(int id);
  Task<User?> Update(UpdateUserRequest model, int id);
}

public class UserService : IUserService
{
  private DBContext ctx = new DBContext();

  public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
  {
    try
    {
      var user = await ctx.users.Where(x => x.Email == model.Email).FirstOrDefaultAsync();

      if (user != null && BCrypt.Verify(model.Password, user.Password))
      {
        var token = generateJwtToken(user);

        return new AuthenticateResponse(user, token);
      }

      return null;
    }
    catch
    {
      throw;
    }
  }

  public async Task<IEnumerable<User?>> Find(FindRequest model)
  {
    try
    {
      return await ctx.users
          .Where(s =>
      EF.Functions.Like(s.FullName, $"%{model.keyword}%") ||
      EF.Functions.Like(s.Email, $"%{model.keyword}%"))
          .Select(user => new User
          {
            Id = user.Id,
            CreatedAt = user.CreatedAt,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            UpdatedAt = user.UpdatedAt
          })
          .Skip(((model.page ?? 1) - 1) * (model.limit ?? 10))
          .Take(model.limit ?? 10)
          .ToListAsync();
    }
    catch
    {
      throw;
    }
  }

  public User? GetById(int id)
  {
    try
    {
      return ctx.users.Where(x => x.Id == id).FirstOrDefault();
    }
    catch (Exception e)
    {
      Console.WriteLine(e.Message);
      return null;
    }
  }

  public async Task<bool?> DeleteById(int id)
  {
    try
    {
      var user = await ctx.users.Where(s => s.Id == id).FirstAsync();
      if (user != null)
      {
        ctx.users.Remove(user);
        await ctx.SaveChangesAsync();
        return true;
      }
      throw new Exception("User does not exist.");
    }
    catch
    {
      throw;
    }
  }

  public async Task<User?> Update(UpdateUserRequest model,int id)
  {
    try
    {
      var user = await ctx.users.Where(x => x.Id == id).FirstOrDefaultAsync(); 
      if (user == null){
        throw new Exception("User does not exist.");
      }
      if (model.Email!=null) user.Email = model.Email;
      if (model.FullName!=null) user.FullName = model.FullName;
      await ctx.SaveChangesAsync();
      return user;
    }
    catch
    {
      throw;
    }
  }


  public async Task<User?> Register(RegisterRequest model)
  {
    try
    {
      if (ctx.users.Any(x => x.Email == model.Email))
        throw new ApplicationException("Email '" + model.Email + "' is already taken");

      var user = new User
      {
        Email = model.Email,
        FullName = model.Fullname,
      };

      user.Password = BCrypt.HashPassword(model.Password);

      await ctx.users.AddAsync(user);
      await ctx.SaveChangesAsync();
      return user;
    }
    catch
    {
      throw;
    }
  }

  private string generateJwtToken(User user)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
      Expires = DateTime.UtcNow.AddDays(7),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
}