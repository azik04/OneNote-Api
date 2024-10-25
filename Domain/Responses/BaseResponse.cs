using Domain.Enum;

namespace Domain.Responses;

public class BaseResponse<T> : IBaseResponse<T>
{
    public StatusCode StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}
