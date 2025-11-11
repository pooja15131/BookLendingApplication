namespace BookLendingApplication.Models;

public class ApiResponse<T>
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public string? Error { get; set; }

    /// <summary>
    /// Success response 
    /// </summary>
    /// <param name="data">response data</param>
    /// <param name="message">message</param>
    /// <param name="code">status code</param>
    /// <returns></returns>
    public static ApiResponse<T> Success(T data, string message = "Success", int code = 200)
    {
        return new ApiResponse<T>
        {
            Code = code,
            Message = message,
            Data = data,
            Error = null
        };
    }

    /// <summary>
    /// Failure response
    /// </summary>
    /// <param name="error">error details</param>
    /// <param name="message">message</param>
    /// <param name="code">status code</param>
    /// <returns></returns>
    public static ApiResponse<T> Failure(string error, string message = "Error", int code = 400)
    {
        return new ApiResponse<T>
        {
            Code = code,
            Message = message,
            Data = default,
            Error = error
        };
    }
}