
using BBTest.Models;

namespace BBTest.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Создать пользователя по емаилу 
        /// </summary>
        Task<User> CreateUser(string email);
        /// <summary>
        /// Получить пользователя
        /// </summary>
        User? GetUser(Guid userId);
        /// <summary>
        /// Отправить депозит
        /// </summary>
        Task<double?> DepositAsync(Guid userId, double amount);
        /// <summary>
        /// Снять деньги
        /// </summary>
        Task<(bool Success, string? Error, double? NewBalance)> WithdrawAsync(Guid userId, double amount);
    }
}