using BBTest.DTOs;
using BBTest.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BBTest.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Produces("application/json")]
    public class UsersController(IUserService service) : ControllerBase
    {
        /// <summary>
        /// Регистрирует нового пользователя.
        /// </summary>
        /// <param name="req">Данные для регистрации (email)</param>
        /// <returns>Информация о пользователе</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserRequest req)
        {
            var user = await service.CreateUser(req.Email);
            return Ok(new UserResponse(user));
        }

        /// <summary>
        /// Получает текущий баланс пользователя.
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Текущий баланс</returns>
        [HttpGet("{userId:guid}/balance")]
        [ProducesResponseType(typeof(UserBalanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserBalanceResponse> GetBalance(Guid userId)
        {
            var user = service.GetUser(userId);
            if (user == null) return NotFound();
            return Ok(new UserBalanceResponse
            {
                UserId = user.UserId,
                Balance = user.Balance
            });
        }

        /// <summary>
        /// Эмулирует пополнение счёта пользователя.
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="req">Сумма пополнения</param>
        /// <returns>Новый баланс</returns>
        [HttpPost("{userId:guid}/deposit")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Deposit(Guid userId, [FromBody] AmountRequest req)
        {
            var result = await service.DepositAsync(userId, req.Amount);
            return result == null
                ? BadRequest("Invalid amount or user not found")
                : Ok(new { userId, newBalance = result });
        }

        /// <summary>
        /// Эмулирует вывод средств с баланса.
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="req">Сумма вывода</param>
        /// <returns>Новый баланс или ошибка</returns>
        [HttpPost("{userId:guid}/withdraw")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Withdraw(Guid userId, [FromBody] AmountRequest req)
        {
            var (success, error, newBalance) = await service.WithdrawAsync(userId, req.Amount);
            return success
                ? Ok(new { userId, newBalance })
                : BadRequest(new { error });
        }
    }
}
