using Domain.Enum;
using Domain.Models.Base;

namespace Domain.Models
{
    public class Users : BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

    }
}
