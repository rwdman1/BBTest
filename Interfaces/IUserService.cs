
using BBTest.Models;

namespace BBTest.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Регистрирует нового пользователя.
        /// </summary>
        Task<User> CreateUser(string email);
        /// <summary>
        /// Получает пользователя.
        /// </summary>
        User? GetUser(Guid userId);
        /// <summary>
        /// Эмулирует пополнение счёта пользователя.
        /// </summary>
        Task<double?> DepositAsync(Guid userId, double amount);
        /// <summary>
        /// Эмулирует вывод средств с баланса.
        /// </summary>
        Task<(bool Success, string? Error, double? NewBalance)> WithdrawAsync(Guid userId, double amount);
    }
}