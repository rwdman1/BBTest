using BBTest.Data;
using BBTest.Interfaces;
using BBTest.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=wallet.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
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