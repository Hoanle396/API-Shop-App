namespace WebAPI.Services;

using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Models;

public interface ICategoryService
{
  Task<Category?> GetById(int id);
  Task<Category?> CreateCategory(CreateCategoryRequest model);

  Task<IEnumerable<Category?>> Find(FindRequest model);
  Task<bool?> DeleteById(int id);

  Task<Category?> Update(CreateCategoryRequest model,int id);
}

public class CategoryService : ICategoryService
{
  private DBContext ctx = new DBContext();


  public async Task<Category?> GetById(int id)
  {
    try
    {
      return await ctx.categories.Where(x => x.Id == id).FirstOrDefaultAsync();
    }
    catch
    {
      throw;
    }
  }
  public async Task<Category?> CreateCategory(CreateCategoryRequest model)
  {
    try
    {
      var category = new Category
      {
        Name = model.Name,
        Description = model.Description,
      };
      await ctx.categories.AddAsync(category);
      await ctx.SaveChangesAsync();
      return category;
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
      var category = await ctx.categories.Where(s => s.Id == id).FirstAsync();
      if (category != null)
      {
        ctx.categories.Remove(category);
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

  public async Task<IEnumerable<Category?>> Find(FindRequest model)
  {
    try
    {
      return await ctx.categories
          .Where(
            s =>
              EF.Functions.Like(s.Description!, $"%{model.keyword}%") ||
              EF.Functions.Like(s.Name!, $"%{model.keyword}%"))
          .Skip(((model.page ?? 1) - 1) * (model.limit ?? 10))
          .Take(model.limit ?? 10)
          .ToListAsync();
    }
    catch
    {
      throw;
    }
  }

  public async Task<Category?> Update(CreateCategoryRequest model, int id)
  {
    try
    {
      
      var category = await ctx.categories.Where(x => x.Id == id).FirstOrDefaultAsync(); 
      if (category == null){
        throw new Exception("Category does not exist.");
      }
      category.Name = model.Name;
      category.Description = model.Description;
      await ctx.SaveChangesAsync();
      return category;
    }
    catch
    {
      throw;
    }
  }
}