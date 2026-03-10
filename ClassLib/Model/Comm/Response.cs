using System.Net;
namespace ClassLib.Models.Comm;

public class Response<T>
{
    public Guid RequestID { get; set; }
    public bool IsSuccess { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public Dictionary<string, string[]>? ValidationErrors { get; set; }
    public T? Data { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow; 
   
}
