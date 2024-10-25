using Domain.Enum;
using Domain.Models.Base;

namespace Domain.Models
{
    public class Users : BaseModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

    }
}
