using BBTest.Data;
using BBTest.Interfaces;
using BBTest.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=wallet.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var users = await db.Users.ToListAsync();
    Console.WriteLine("Список пользователей:");
    foreach (var user in users)
    {
        Console.WriteLine($"ID: {user.UserId}, Email: {user.Email}, Balance: {user.Balance}");
    }
}

app.Run();