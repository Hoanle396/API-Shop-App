namespace WebAPI.Services;

using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Helpers;
using WebAPI.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Drawing;

public interface IProductService {
    Task<dynamic?> CreateProduct(CreateProductRequest model,List<Models.Size> sizes);
    Task<IEnumerable<Product?>> Find(FindRequest model);
    Task<dynamic?> GetById(int id);
    Task<bool?> DeleteById(int id);
}

public class ProductService : IProductService {
    private DBContext ctx = new DBContext();
    private Cloudinary cloudinary = new Cloudinary(AppSettings.CLOUD_URL);

    public ProductService() {
        cloudinary.Api.Secure = true;
    }
    public async Task<dynamic?> CreateProduct(CreateProductRequest model, List<Models.Size> sizes) {
        using (var dbcxtransaction = ctx.Database.BeginTransaction()) {
            try {
                var category = await new CategoryService().GetById(model.category);
                if (category == null) {
                    throw new Exception("Could not find category");
                }
                var product = new Product {
                    Name = model.name,
                    Description = model.description,
                    Price = model.price,
                    categoryId = model.category
                };
                await ctx.products.AddAsync(product);
                await ctx.SaveChangesAsync();

                foreach (var size in sizes) {
                    await ctx.sizes.AddAsync(new Entities.Size {
                        SizeName = size.sizeName,
                        Quantity = size.quantity,
                        productId = product.Id
                    });
                    await ctx.SaveChangesAsync();
                }
              foreach (var image in model.images ?? new List<IFormFile>()) {
                    var uploadResult = new ImageUploadResult();
                    if (image.Length > 0) {
                        using (var filestream = image.OpenReadStream()) {
                            var uploadParams = new ImageUploadParams {
                                File = new FileDescription(image.FileName, filestream),

                            };
                            uploadResult = cloudinary.Upload(uploadParams);
                        }
                    }

                    if (uploadResult.Error != null) {
                        throw new Exception(uploadResult.Error.Message);
                    }
                    await ctx.images.AddAsync(new Image {
                        productId = product.Id,
                        Url = uploadResult.SecureUrl.AbsoluteUri,
                    });
                    await ctx.SaveChangesAsync();
                }

                dbcxtransaction.Commit();
                product.category = category;
                return product;
            } catch {
                dbcxtransaction.Rollback();
                throw;
            }
        }
    }

    public async Task<bool?> DeleteById(int id) {
        try {
            var product= await ctx.products.Where(s => s.Id == id).FirstAsync();
            if (product != null) {
                ctx.products.Remove(product);
                await ctx.SaveChangesAsync();
                return true;
            }
            throw new Exception("Product does not exist.");
        } catch {
            throw;
        }
    }

    public async Task<IEnumerable<Product?>> Find(FindRequest model) {
        try {
            return await ctx.products
                .Where(s =>
            EF.Functions.Like(s.Description, $"%{model.keyword}%") ||
            EF.Functions.Like(s.Name, $"%{model.keyword}%"))
                .Select(product => new Product {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    category = product.category,
                    Description = product.Description,
                    CreatedAt = product.CreatedAt,
                    Images = product.Images!.ToList() ?? new List<Image>(),
                    Sizes = product.Sizes!.ToList() ?? new List<Entities.Size>(),
                })
                .Skip(((model.page ?? 1) - 1) * (model.limit ?? 10))
                .Take(model.limit ?? 10)
                .ToListAsync();
        } catch {
            throw;
        }
    }

    public async Task<dynamic?> GetById(int id) {
        try {
            return await ctx.products.Where(s => s.Id == id).Select(product => new {
                id = product.Id,
                name = product.Name,
                price = product.Price,
                category = product.category,
                description = product.Description,
                createdAt = product.CreatedAt,
                images = product.Images!.ToList() ?? new List<Image>(),
                sizes = product.Sizes!.ToList() ?? new List<Entities.Size>(),
            }).FirstAsync();
        } catch {
            throw;
        }
    }
}