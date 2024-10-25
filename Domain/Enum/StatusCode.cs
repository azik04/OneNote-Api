namespace Domain.Enum;

public enum StatusCode
{
    NotFound = 10,
    OK = 200,
    InternalServerError = 500,
    UserNotFound = 0,
    UserAlreadyExists = 1,
    WrongPassword = 2,
    WrongChangePassword = 3
}
