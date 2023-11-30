using WebAPI.Entities;
using WebAPI.Enum;

namespace WebAPI.Models;

public class AuthenticateResponse {
    public int Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public Roles Role { get; set; }
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public string Token { get; set; }


    public AuthenticateResponse(User user, string token) {
        Id = user.Id;
        FullName = user.FullName!;
        Email = user.Email!;
        Role = user.Role;
        CreatedAt = user.CreatedAt;
        UpdatedAt = user.UpdatedAt;
        Token = token;
    }
}