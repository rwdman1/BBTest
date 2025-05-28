using BBTest.Data;
using BBTest.Interfaces;
using BBTest.Models;
using System.ComponentModel.DataAnnotations;

namespace BBTest.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User> CreateUser(string email)
        {
            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email address", nameof(email));
            var user = new User { Email = email };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            Console.WriteLine($"CreateUser {user.Email} id {user.UserId}");
            return user;
        }

        public User? GetUser(Guid userId) =>
            _db.Users.FirstOrDefault(u => u.UserId == userId);

        public async Task<double?> DepositAsync(Guid userId, double amount)
        {
            if (amount <= 0) return null;
            var user = GetUser(userId);
            if (user == null) return null;
            user.Balance += amount;
            await _db.SaveChangesAsync();
            Console.WriteLine($"Deposit {user.Email} balanse {user.Balance}");
            return user.Balance;
        }

        public async Task<(bool Success, string? Error, double? NewBalance)> WithdrawAsync(Guid userId, double amount)
        {
            if (amount <= 0) return (false, "Invalid amount", null);
            var user = GetUser(userId);
            if (user == null) return (false, "User not found", null);
            if (user.Balance < amount)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Withdraw {user.Email} id {user.Balance} amount {amount} ERROR Invalid amount");
                Console.ForegroundColor = ConsoleColor.White;
                return (false, "Insufficient funds", null);
            }
            user.Balance -= amount;
            await _db.SaveChangesAsync();
            Console.WriteLine($"Withdraw {user.Email} balance {user.Balance} amount {amount}");
            return (true, null, user.Balance);
        }
        /// <summary>
        /// проверка валидности почты
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsValidEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }
    }
}
