using System.ComponentModel.DataAnnotations;

namespace BBTest.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Required]
        public string Email { get; set; } = string.Empty;

        public double Balance { get; set; } = 0;
    }
}
