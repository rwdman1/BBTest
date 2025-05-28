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
        /// ������������ ������ ������������.
        /// </summary>
        /// <param name="req">������ ��� ����������� (email)</param>
        /// <returns>���������� � ������������</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserRequest req)
        {
            var user = await service.CreateUser(req.Email);
            return Ok(new UserResponse(user));
        }

        /// <summary>
        /// �������� ������� ������ ������������.
        /// </summary>
        /// <param name="userId">ID ������������</param>
        /// <returns>������� ������</returns>
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
        /// ��������� ���������� ����� ������������.
        /// </summary>
        /// <param name="userId">ID ������������</param>
        /// <param name="req">����� ����������</param>
        /// <returns>����� ������</returns>
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
        /// ��������� ����� ������� � �������.
        /// </summary>
        /// <param name="userId">ID ������������</param>
        /// <param name="req">����� ������</param>
        /// <returns>����� ������ ��� ������</returns>
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
