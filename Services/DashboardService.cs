namespace WebAPI.Services;

using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Models;

public interface IDashboardService
{
  Task<dynamic?> Statistical();

  Task<dynamic?> WeeklyReport();
}

public class DashboardService : IDashboardService
{
  private DBContext ctx = new DBContext();


  public async Task<dynamic?> Statistical()
  {
    try
    {
      var month = DateTime.Today.AddDays(-30);
      return new
      {
        products = new
        {
          total = await ctx.products.CountAsync(),
          month = await ctx.products.Where(s => s.CreatedAt >= month).CountAsync()

        },
        users = new
        {
          total = await ctx.users.CountAsync(),
          month = await ctx.users.Where(s => s.CreatedAt >= month).CountAsync()
        },
        orders = new
        {
          total = await ctx.orders.CountAsync(),
          month = await ctx.orders.Where(s => s.CreatedAt >= month).CountAsync()
        },
        sales = new
        {
          total = await ctx.orders.SumAsync(i => i.Amount),
          month = await ctx.orders.Where(s => s.CreatedAt >= month).SumAsync(i => i.Amount)
        }
      };
    }
    catch
    {
      throw;
    }
  }
  public async Task<dynamic?> WeeklyReport()
  {
    try
    {
      return new
      {
        orders = ctx.orders.GroupBy(s => s.CreatedAt.Date)
        .Select(g => new
        {
          date = g.Key,
          value = g.Count()
        }).OrderByDescending(s => s.date).Take(7).Reverse(),

        sales = ctx.orders.GroupBy(s => s.CreatedAt.Date)
        .Select(g => new
        {
          date = g.Key,
          value = g.Sum(s => s.Amount)
        }).OrderByDescending(s => s.date).Take(7).Reverse()
      };
    }
    catch
    {
      throw;
    }
  }

}