using Boxer.Data;
using Boxer.Management.Security.Authentication.api;
using Boxer.Management.Security.Authentication.Data;
using Boxer.Management.Security.Authentication.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using db = Boxer.Management.Security.Authentication.Data;

var builder = WebApplication.CreateBuilder(args);
var allowOriginName = "boxer.client";
builder.Services.AddDbContext<db.DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbConn") 
    ?? throw new InvalidOperationException("Database connection string not found.")));
builder.Services.AddIdentityApiEndpoints<UserAccount>().AddEntityFrameworkStores<db.DatabaseContext>();
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
 //   .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IPasswordHasher<UserAccount>, PasswordHasher<UserAccount>>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowOriginName,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:8100",
                                              "https://localhost:8101").AllowAnyHeader()
                                                  .AllowAnyMethod().SetIsOriginAllowedToAllowWildcardSubdomains(); ;
                      });
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
//app.UseAuthentication();
app.UseCors(allowOriginName);
//app.UseAuthorization();

app.MapControllers();

app.Run();
