using Domain.Enum;

namespace Domain.Responses;

public interface IBaseResponse<T>
{
    public StatusCode StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}
