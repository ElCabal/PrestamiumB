using Microsoft.EntityFrameworkCore;
using Prestamium.Persistence;
using Prestamium.Repositories.Interfaces;
using Prestamium.Repositories.Repositories;
using Prestamium.Services.Interfaces;
using Prestamium.Services.Profiles;
using Prestamium.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
});

//register services

builder.Services.AddTransient<IClientRepository, ClientRepository>();

builder.Services.AddTransient<IClientService, ClientService>();

//Profile Mappers
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<ClientProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
