using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Prestamium.Persistence;
using Prestamium.Repositories.Interfaces;
using Prestamium.Repositories.Repositories;
using Prestamium.Services.Interfaces;
using Prestamium.Services.Profiles;
using Prestamium.Services.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar Identity
builder.Services.AddIdentityCore<PrestamiumUserIdentity>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configurar JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});

//Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//register service

builder.Services.AddTransient<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IClientService, ClientService>();
builder.Services.AddTransient<ILoanRepository, LoanRepository>();
builder.Services.AddTransient<ILoanService, LoanService>();
builder.Services.AddTransient<IInstallmentRepository, InstallmentRepository>();
builder.Services.AddScoped<IBoxService, BoxService>();
builder.Services.AddScoped<IBoxRepository, BoxRepository>();
builder.Services.AddScoped<IBoxTransactionRepository, BoxTransactionRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

//Profile Mappers
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<ClientProfile>();
    config.AddProfile<LoanProfile>();
    config.AddProfile<BoxProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
