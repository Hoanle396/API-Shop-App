namespace WebAPI.Helpers
{
  public class Utils
  {
    private static Random random = new Random();

    public static string RandomCode()
    {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      return new string(Enumerable.Repeat(chars, 8)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
  }
}
