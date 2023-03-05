using Microsoft.EntityFrameworkCore;
using webapi_hospital.Models;
using webapi_hospital.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISha256Encoder, Sha256Encoder>();

builder.Services.AddDbContext<HospitalDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("HospitalDb"));
});

builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("api/v1/user/get/{id:int}", async (int id, IUserRepository userManager) =>
{
    if (await userManager.GetByIdAsync(id) is { } user)
    {
        return Results.Json(user);
    }
    return Results.BadRequest();
});

app.MapGet("api/v1/user/get", async (IUserRepository userManager) =>
{
    if (await userManager.GetAsync() is { } users)
    {
        return Results.Json(users);
    }
    return Results.BadRequest();
});

app.MapPost("api/v1/user/create", async (User user, IUserRepository userManager, ISha256Encoder encoder) =>
{
    user.Password = encoder.ComputeSha256Hash(user.Password);
    
    if (await userManager.AddAsync(user))
    {
        return Results.Ok();
    }
    return Results.BadRequest();
});

app.MapDelete("api/v1/user/delete/{id:int}", async (int id, IUserRepository userManager) =>
{
    if (await userManager.RemoveAsync(id))
    {
        return Results.Ok();
    }
    return Results.BadRequest();
});

app.UseCors(x =>
{
    x.AllowAnyMethod();
    x.AllowAnyOrigin();
    x.AllowAnyHeader();
});

app.Run();