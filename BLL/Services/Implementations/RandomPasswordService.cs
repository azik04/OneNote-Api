using BLL.Services.Interfaces;
using Domain.Responses;
using System.Text;
using Serilog;

namespace BLL.Services.Implementations;

public class RandomPasswordService : IRandomPasswordService
{
    public async Task<IBaseResponse<string>> GeneratePasswordAsync(int length = 16)
    {
        try
        {
            Log.Information("Generating a random password with length {Length}.", length);

            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string specialChars = "!@#$%^&*()_+[]{}";

            var random = new Random();
            var password = new StringBuilder();

            password.Append(upperChars[random.Next(upperChars.Length)]);
            password.Append(lowerChars[random.Next(lowerChars.Length)]);
            password.Append(numbers[random.Next(numbers.Length)]);
            password.Append(specialChars[random.Next(specialChars.Length)]);

            string allChars = upperChars + lowerChars + numbers + specialChars;

            for (int i = password.Length; i < length; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            var randomPassword = new string(password.ToString().OrderBy(x => random.Next()).ToArray());

            Log.Information("Password generated successfully: {RandomPassword}.", randomPassword);

            return new BaseResponse<string>
            {
                Data = randomPassword,
                StatusCode = Domain.Enum.StatusCode.OK,
                Message = "Password generated successfully."
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to generate password. Error: {ErrorMessage}", ex.Message);
            return new BaseResponse<string>
            {
                StatusCode = Domain.Enum.StatusCode.InternalServerError,
                Message = $"Failed to generate password: {ex.Message}"
            };
        }
    }
}
