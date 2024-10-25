using BLL.Services.Interfaces;
using DAL.Repositories;
using Domain.DTO_s.Users;
using Domain.Enum;
using Domain.Models;
using Domain.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Serilog;

namespace BLL.Services.Implementations;

public class UserService : IUserService
{
    private readonly IBaseRepository<Users> _rep;

    public UserService(IBaseRepository<Users> rep)
    {
        _rep = rep;
    }

    public async Task<IBaseResponse<GetUserDTO>> Create(CreateUserDTO vm)
    {
        try
        {
            var existingUser = await _rep.GetAll().SingleOrDefaultAsync(x => x.Email == vm.Email);
            if (existingUser != null)
            {
                Log.Warning("User creation failed: User with email {Email} already exists.", vm.Email);
                return new BaseResponse<GetUserDTO>
                {
                    Message = "User already exists with the provided email.",
                    StatusCode = Domain.Enum.StatusCode.UserAlreadyExists,
                };
            }

            var newUser = new Users()
            {
                CreateAt = DateTime.UtcNow,
                Email = vm.Email,
                FullName = $"{vm.UserName} {vm.UserSurname}",
                Password = vm.Password,
                Role = Domain.Enum.Role.User,
            };

            await _rep.Create(newUser);
            Log.Information("User created successfully with ID {UserId}.", newUser.Id);

            var dto = new GetUserDTO()
            {
                id = newUser.Id,
                Email = newUser.Email,
                FullName = newUser.FullName,
            };

            return new BaseResponse<GetUserDTO>
            {
                Data = dto,
                Message = "User created successfully.",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while creating a user.");
            return new BaseResponse<GetUserDTO>
            {
                Message = "An error occurred while creating the user: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<GetUserDTO>> Delete(long id)
    {
        try
        {
            if (id <= 0)
            {
                Log.Warning("Delete failed: Invalid user ID {UserId}.", id);
                return new BaseResponse<GetUserDTO>
                {
                    Message = "Invalid user ID.",
                    StatusCode = Domain.Enum.StatusCode.UserNotFound
                };
            }

            var userToDelete = await _rep.GetAll().SingleOrDefaultAsync(x => x.Id == id);
            if (userToDelete == null)
            {
                Log.Warning("Delete failed: User with ID {UserId} not found.", id);
                return new BaseResponse<GetUserDTO>
                {
                    Message = "User not found.",
                    StatusCode = Domain.Enum.StatusCode.UserNotFound
                };
            }

            userToDelete.IsDeleted = true;
            userToDelete.DeleteAt = DateTime.UtcNow;
            Log.Information("User with ID {UserId} marked as deleted.", userToDelete.Id);

            var vm = new GetUserDTO()
            {
                id = userToDelete.Id,
                Email = userToDelete.Email,
                FullName = userToDelete.FullName,
            };

            return new BaseResponse<GetUserDTO>
            {
                Data = vm,
                Message = "User deleted successfully.",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while deleting a user.");
            return new BaseResponse<GetUserDTO>
            {
                Message = "An error occurred while deleting the user: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<ICollection<GetUserDTO>>> GetAll()
    {
        try
        {
            var users = await _rep.GetAll()
               .Where(x => !x.IsDeleted)
               .Select(item => new GetUserDTO
               {
                   id = item.Id,
                   Email = item.Email,
                   FullName = item.FullName,
               })
               .ToListAsync();

            Log.Information("Retrieved all users. Count: {Count}", users.Count);
            return new BaseResponse<ICollection<GetUserDTO>>()
            {
                Data = users,
                Message = "Users retrieved successfully.",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving all users.");
            return new BaseResponse<ICollection<GetUserDTO>>()
            {
                Message = "An error occurred while retrieving users: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<GetUserDTO>> GetById(long id)
    {
        try
        {
            var user = await _rep.GetAll().SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (user == null)
            {
                Log.Warning("GetById failed: User with ID {UserId} not found.", id);
                return new BaseResponse<GetUserDTO>
                {
                    Message = "User not found.",
                    StatusCode = Domain.Enum.StatusCode.UserNotFound
                };
            }

            var vm = new GetUserDTO()
            {
                Email = user.Email,
                FullName = user.FullName,
                id = user.Id
            };

            Log.Information("User with ID {UserId} retrieved successfully.", user.Id);
            return new BaseResponse<GetUserDTO>()
            {
                Data = vm,
                Message = "User retrieved successfully.",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving user by ID.");
            return new BaseResponse<GetUserDTO>
            {
                Message = "An error occurred while retrieving the user: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<GetUserDTO>> Update(long id, UpdateUserDTO vm)
    {
        try
        {
            var userToUpdate = await _rep.GetAll().SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (userToUpdate == null)
            {
                Log.Warning("Update failed: User with ID {UserId} not found.", id);
                return new BaseResponse<GetUserDTO>
                {
                    StatusCode = Domain.Enum.StatusCode.NotFound,
                    Message = "User not found."
                };
            }

            userToUpdate.FullName = vm.FullName;
            userToUpdate.Email = vm.Email;

            await _rep.Update(userToUpdate);
            Log.Information("User with ID {UserId} updated successfully.", userToUpdate.Id);

            var dto = new GetUserDTO()
            {
                FullName = userToUpdate.FullName,
                Email = userToUpdate.Email,
            };

            return new BaseResponse<GetUserDTO>
            {
                Data = dto,
                Message = "User updated successfully.",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while updating user.");
            return new BaseResponse<GetUserDTO>
            {
                Message = "An error occurred while updating the user: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<string>> LogIn(LogInDTO vm)
    {
        try
        {
            var user = await _rep.GetAll()
                .FirstOrDefaultAsync(x => x.FullName == vm.FullName && !x.IsDeleted);

            if (user == null || vm.Password != user.Password)
            {
                Log.Warning("Login failed: Invalid username or password for {Username}.", vm.FullName);
                return new BaseResponse<string>
                {
                    Message = "Invalid username or password.",
                    StatusCode = Domain.Enum.StatusCode.UserNotFound
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("0BD0E95C-6387-4135-A80E-489FF6E5C1DF");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            Log.Information("User {Username} logged in successfully.", user.FullName);
            return new BaseResponse<string>
            {
                Message = "Login successful.",
                StatusCode = Domain.Enum.StatusCode.OK,
                Data = tokenString
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred during user login.");
            return new BaseResponse<string>
            {
                Message = "An error occurred during login: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<GetUserDTO>> ChangeRole(long id, Role role)
    {
        try
        {
            var user = await _rep.GetAll().SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (user == null)
            {
                Log.Warning("ChangeRole failed: User with ID {UserId} not found.", id);
                return new BaseResponse<GetUserDTO>
                {
                    Message = "User not found.",
                    StatusCode = Domain.Enum.StatusCode.UserNotFound
                };
            }

            user.Role = role;
            await _rep.Update(user);
            Log.Information("User with ID {UserId} role changed to {Role}.", id, role);

            var dto = new GetUserDTO()
            {
                id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
            };

            return new BaseResponse<GetUserDTO>
            {
                Data = dto,
                Message = "User role updated successfully.",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while changing user role.");
            return new BaseResponse<GetUserDTO>
            {
                Message = "An error occurred while changing the user role: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<GetUserDTO>> ChangePassword(long id, ChangePasswordDTO vm)
    {
        try
        {
            if (id <= 0)
            {
                Log.Warning("ChangePassword failed: Invalid user ID {UserId}.", id);
                return new BaseResponse<GetUserDTO>
                {
                    Message = "Invalid user ID.",
                    StatusCode = Domain.Enum.StatusCode.UserNotFound
                };
            }

            var user = await _rep.GetAll().SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (user == null)
            {
                Log.Warning("ChangePassword failed: User with ID {UserId} not found.", id);
                return new BaseResponse<GetUserDTO>
                {
                    Message = "User not found.",
                    StatusCode = Domain.Enum.StatusCode.UserNotFound
                };
            }

            if (vm.OldPassword != user.Password)
            {
                Log.Warning("ChangePassword failed: Invalid old password for user ID {UserId}.", id);
                return new BaseResponse<GetUserDTO>
                {
                    Message = "Invalid old password.",
                    StatusCode = Domain.Enum.StatusCode.WrongPassword
                };
            }

            user.Password = vm.NewPassword;

            await _rep.Update(user);

            Log.Information("User with ID {UserId} changed their password successfully.", id);

            var dto = new GetUserDTO()
            {
                id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
            };

            return new BaseResponse<GetUserDTO>
            {
                Data = dto,
                Message = "Password changed successfully.",
                StatusCode = Domain.Enum.StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while changing password for user ID {UserId}.", id);
            return new BaseResponse<GetUserDTO>
            {
                Message = "An error occurred while changing the password: " + ex.Message,
                StatusCode = Domain.Enum.StatusCode.InternalServerError
            };
        }
    }

}
