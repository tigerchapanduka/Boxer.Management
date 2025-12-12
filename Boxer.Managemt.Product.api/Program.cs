using Boxer.Data;
using Boxer.Management.Product.Data;
using Boxer.Management.Service;
using Boxer.Managemt.Product.api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using Db = Boxer.Management.Product.Data;
/*
 * use for Db migrations
using DatabaseContext = Boxer.Managemt.Product.api.DatabaseContext;
*/

var builder = WebApplication.CreateBuilder(args);
var allowOriginName = "boxer.client";


builder.Services.AddDbContext<Db.DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbConn") 
    ?? throw new InvalidOperationException("Database connection string not found.")));
 /*
  * Migration db context
   builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbConn")
    ?? throw new InvalidOperationException("Database connection string not found.")
    ));
*/

builder.Services.AddAuthorization();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowOriginName,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:8101",
                                              "https://localhost:8101").AllowAnyHeader()
                                                  .AllowAnyMethod().SetIsOriginAllowedToAllowWildcardSubdomains(); ;
                      });
});

builder.Services.AddAuthentication(cfg => {
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8
            .GetBytes(builder.Configuration["AppSettings:JWT_Secret"])
        ),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseCors(allowOriginName);
app.UseAuthorization();
//app.MapGroup("auth").MapIdentityApi<UserAccount>();
app.MapControllers();

app.Run();
