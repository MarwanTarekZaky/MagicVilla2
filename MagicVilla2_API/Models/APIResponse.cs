using System.Net;

namespace MagicVilla2_API.Models;

public class APIResponse
{
    public bool IsSuccess { get; set; } = true;
    public List<string> ErrorMessage { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public object Result { get; set; }
    
}