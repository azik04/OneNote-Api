using Domain.DTO_s.Users;
using Domain.Enum;
using Domain.Responses;

namespace BLL.Services.Interfaces;

public interface IUserService
{
    public Task<IBaseResponse<GetUserDTO>> Create(CreateUserDTO vm);
    public Task<IBaseResponse<ICollection<GetUserDTO>>> GetAll();
    public Task<IBaseResponse<GetUserDTO>> GetById(long id);
    public Task<IBaseResponse<GetUserDTO>> Delete(long id);
    public Task<IBaseResponse<GetUserDTO>> Update(long id ,UpdateUserDTO vm);
    public Task<IBaseResponse<GetUserDTO>> ChangeRole(long id, Role role);
    public Task<IBaseResponse<GetUserDTO>> ChangePassword(long id, ChangePasswordDTO vm);
    public Task<IBaseResponse<string>> LogIn(LogInDTO vm);
}
