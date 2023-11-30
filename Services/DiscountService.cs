namespace WebAPI.Services;

using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Models;

public interface IDiscountService
{
  Task<Discount?> GetById(int id);
  Task<Discount?> Create(CreateDiscountRequest model);

  Task<IEnumerable<Discount?>> Find(FindRequest model);
  Task<bool?> DeleteById(int id);
  Task<IEnumerable<Discount?>> FindWork(FindRequest model);
  Task<Discount> Check(string code);
}

public class DiscountService : IDiscountService
{
  private DBContext ctx = new DBContext();


  public async Task<Discount?> GetById(int id)
  {
    try
    {
      return await ctx.discounts.Where(x => x.Id == id).FirstOrDefaultAsync();
    }
    catch
    {
      throw;
    }
  }
  public async Task<Discount?> Create(CreateDiscountRequest model)
  {
    try
    {

      var discount = new Discount
      {
        Code = model.code,
        Value = model.value,
        EndDate = model.endDate,
        Type = model.type,
        Description = model.description,
      };
      await ctx.discounts.AddAsync(discount);
      await ctx.SaveChangesAsync();
      return discount;
    }
    catch
    {
      throw;
    }
  }

  public async Task<bool?> DeleteById(int id)
  {
    try
    {
      var discount = await ctx.discounts.Where(s => s.Id == id).FirstAsync();
      if (discount != null)
      {
        ctx.discounts.Remove(discount);
        await ctx.SaveChangesAsync();
        return true;
      }
      throw new Exception("Category does not exist.");
    }
    catch
    {
      throw;
    }
  }

  public async Task<IEnumerable<Discount?>> Find(FindRequest model)
  {
    try
    {
      return await ctx.discounts
          .Where(
            s =>
              EF.Functions.Like(s.Description!, $"%{model.keyword}%"))
          .Skip(((model.page ?? 1) - 1) * (model.limit ?? 10))
          .Take(model.limit ?? 10)
          .ToListAsync();
    }
    catch
    {
      throw;
    }
  }
  public async Task<IEnumerable<Discount?>> FindWork(FindRequest model)
  {
    try
    {
      return await ctx.discounts
          .Where(
            s =>
              EF.Functions.Like(s.Description!, $"%{model.keyword}%") && s.EndDate > DateTime.Now)
          .Skip(((model.page ?? 1) - 1) * (model.limit ?? 10))
          .Take(model.limit ?? 10)
          .ToListAsync();
    }
    catch
    {
      throw;
    }
  }
  public async Task<Discount> Check(string code)
  {
    try
    {
      var discount = await ctx.discounts
          .Where(
            s => s.Code == code && s.EndDate > DateTime.Now)
          .FirstOrDefaultAsync();
      if (discount == null)
      {
        throw new Exception("Discount not found or expired");
      }
      return discount;
    }
    catch
    {
      throw;
    }
  }
}