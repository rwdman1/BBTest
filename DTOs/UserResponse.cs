using BBTest.Models;

namespace BBTest.DTOs
{
    public class UserResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public double Balance { get; set; }
        public UserResponse(User user)
        {
            UserId = user.UserId;
            Email = user.Email;
            Balance = user.Balance;
        }
    }
}
