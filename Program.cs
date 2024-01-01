using dotenv.net;
using Microsoft.OpenApi.Models;
using WebAPI.Helpers;
using WebAPI.Services;
DotEnv.Load(options: new DotEnvOptions(ignoreExceptions: false));
var builder = WebApplication.CreateBuilder(args);

//await new DBContext().DeleteDatabase();
//await new DBContext().CreateDatabase();
// Add services to the container.

{
    var services = builder.Services;
    services.AddCors();
    services.AddControllers();

    // configure DI for application services
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<ICategoryService, CategoryService>();
    services.AddScoped<IProductService, ProductService>();
    services.AddScoped<IDiscountService, DiscountService>();
    services.AddScoped<IOrderService, OrderService>();
    services.AddScoped<IDashboardService, DashboardService>();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(option => {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "C# API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
  });
    });
}

var app = builder.Build(); {
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
