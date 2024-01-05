namespace WebAPI.Services;

using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Models;
using WebAPI.Enum;
using WebAPI.Helpers;

public interface IOrderService
{
  Task<dynamic?> Create(CreateOrderRequest model, User user);
  Task<IEnumerable<Order?>> Find(FindRequest model, User user);
  Task<dynamic?> GetById(int id);
  Task<bool?> Update(int id, UpdateOrderStatus model);
}

public class OrderService : IOrderService
{
  private IDiscountService discountService = new DiscountService();
  private DBContext ctx = new DBContext();


  public async Task<dynamic?> Create(CreateOrderRequest model, User user)
  {
    using (var dbcxtransaction = ctx.Database.BeginTransaction())
    {
      try
      {
        float price = 0;
        foreach (var productId in model.products)
        {
          var product = await ctx.products.Where(s => s.Id == productId.product).FirstOrDefaultAsync();
          if (product == null)
          {
            throw new Exception("Product does not exist.");
          }
          System.Console.WriteLine($"product: {product}");
          var size = await ctx.sizes.Where(s => s.SizeName == productId.size && s.productId == product.Id).FirstOrDefaultAsync();
          System.Console.WriteLine($"size: {size}");
          if (size == null || (size?.Quantity ?? 0 - productId.quantity) < 0)
          {
            throw new Exception("Product does allow this quantity");
          }
          price += product.Price * productId.quantity;
        }
        if (model.discount != null)
        {
          try
          {
            var discount = await discountService.Check(model.discount);
            if (discount.Type == Enum.Discount.VALUE)
            {
              price -= discount.Value;
            }
            else if (discount.Type == Enum.Discount.PERCENT)
            {
              price -= price * discount.Value / 100;
            }
          }
          catch
          {

          }
        }
        var order = new Order
        {
          Amount = price,
          Status = OrderStatus.PENDING,
          userId = user.Id,
          Code = Utils.RandomCode()
        };
        await ctx.orders.AddAsync(order);
        await ctx.SaveChangesAsync();

        foreach (var productId in model.products)
        {
          var product = await ctx.products.Where(s => s.Id == productId.product).FirstOrDefaultAsync();
          var size = product!.Sizes?.Where(s => s.SizeName == productId.size).FirstOrDefault();
          price += product!.Price * productId.quantity;
          size!.Quantity = size.Quantity - productId.quantity;
          await ctx.SaveChangesAsync();
          var orderDetail = new OrderDetail
          {
            orderId = order.Id,
            productId = product.Id,
            Quantity = productId.quantity,
            Size = productId.size,
            Price = product.Price,
          };
          await ctx.orderDetails.AddAsync(orderDetail);
          await ctx.SaveChangesAsync();
        }

        var shipping = new Shipping
        {
          Address = model.address,
          PaymentStatus = PaymentStatus.PENDING,
          orderId = order.Id,
          Email = model.email,
          Phone = model.phone
        };
        await ctx.shipping.AddAsync(shipping);
        await ctx.SaveChangesAsync();

        dbcxtransaction.Commit();
        return order;
      }
      catch
      {
        dbcxtransaction.Rollback();
        throw;
      }
    }
  }

  public async Task<bool?> Update(int id, UpdateOrderStatus model)
  {
    try
    {
      var order = await ctx.orders.Where(s => s.Id == id).FirstAsync();
      if (order != null)
      {
        order.Status = model.status;
        await ctx.SaveChangesAsync();
        return true;
      }
      throw new Exception("Order does not exist.");
    }
    catch
    {
      throw;
    }
  }

  public async Task<IEnumerable<Order?>> Find(FindRequest model, User user)
  {
    try
    {
      return await ctx.orders
      .Where(s => user.Role != Roles.ADMIN ? s.userId == user.Id : 1 == 1)
      .Select(order => new Order
      {
        Id = order.Id,
        Amount = order.Amount,
        Code = order.Code,
        Status = order.Status,
        CreatedAt = order.CreatedAt,
        UpdatedAt = order.UpdatedAt,
        user = order.user

      })
          .Skip(((model.page ?? 1) - 1) * (model.limit ?? 10))
          .Take(model.limit ?? 10)
          .OrderByDescending(s => s.CreatedAt)
          .ToListAsync();
    }
    catch
    {
      throw;
    }
  }

  public async Task<dynamic?> GetById(int id)
  {
    try
    {
      return await ctx.orders.Where(s => s.Id == id).Select(order => new
      {
        id = order.Id,
        code = order.Code,
        amount = order.Amount,
        status = order.Status,
        user = order.user,
        updatedAt = order.UpdatedAt,
        createdAt = order.CreatedAt,
        detail = ctx.orderDetails.Where(s => s.orderId == order.Id).Select(d => new
        {
          id = d.Id,
          quantity = d.Quantity,
          createdAt = d.CreatedAt,
          size = d.Size,
          updatedAt = d.UpdatedAt,
          product = ctx.products.Where(p => p.Id == d.productId).Select(product => new
          {
            id = product.Id,
            name = product.Name,
            price = product.Price,
            category = product.category,
            description = product.Description,
            createdAt = product.CreatedAt,
            images = product.Images!.ToList() ?? new List<Image>(),
            sizes = product.Sizes!.ToList() ?? new List<Entities.Size>(),
          }).FirstOrDefault(),
        }).ToList(),
        shipping = ctx.shipping.Where(s => s.orderId == order.Id).FirstOrDefault(),
      }).FirstOrDefaultAsync();
    }
    catch
    {
      throw;
    }
  }
}