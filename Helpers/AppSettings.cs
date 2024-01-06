namespace WebAPI.Helpers;
using dotenv.net;
public static class AppSettings
{
  public static readonly string Secret = Environment.GetEnvironmentVariable("JWT_SECRET");
  public static readonly string Connect = Environment.GetEnvironmentVariable("DB_URL");
  public static readonly string CLOUD_URL = $"cloudinary://{Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY")}:{Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")}@{Environment.GetEnvironmentVariable("CLOUDINARY_NAME")}";

  public static readonly string Email = Environment.GetEnvironmentVariable("USER_EMAIL");

  public static readonly string Password = Environment.GetEnvironmentVariable("PASSWORD");
}