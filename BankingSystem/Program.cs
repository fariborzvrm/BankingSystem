
using BankingSystem.API.Extensions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Application.Mappings;
using BankingSystem.Application.Services;
using BankingSystem.Infrastructure.Identity;
using BankingSystem.Infrastructure.Persistence;
using BankingSystem.Infrastructure.Repositories;
using BankingSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using BankingSystem.API.Validators;
using BankingSystem.Api.Exceptions;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerDocumentation();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddJwtAuthentication(builder.Configuration);


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(BankAccountProfile).Assembly);
});


builder.Services.AddValidatorsFromAssemblyContaining<BankAccountValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddMemoryCache();


builder.Services.AddHttpClient<IBranchService, BranchService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BranchesApi:BaseUrl"]!);
    client.DefaultRequestHeaders.Add("X-API-Key", builder.Configuration["BranchesApi:ApiKey"]!);
});



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await DbSeeder.SeedRolesAsync(roleManager);
}

app.Run();
