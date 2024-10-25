using Domain.Responses;

namespace BLL.Services.Interfaces;

public interface IRandomPasswordService
{
    public Task<IBaseResponse<string>> GeneratePasswordAsync(int length = 16);
}
