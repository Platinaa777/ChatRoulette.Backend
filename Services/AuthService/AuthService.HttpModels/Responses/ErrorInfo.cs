namespace AuthService.HttpModels.Responses;

public class ErrorInfo
{
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }

    public ErrorInfo(string errorCode, string errorMessage)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}