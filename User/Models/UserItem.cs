using Npgsql.TypeMapping;

namespace User.Models
{
    public class UserItem
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public UserType UserType { get; set; } = UserType.Customer;
    }
}