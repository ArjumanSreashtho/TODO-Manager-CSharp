namespace Todo_Manager.Helper;

public class CustomException : Exception
{
    public int ErrorCode { get; }

    public CustomException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}