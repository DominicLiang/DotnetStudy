namespace Test2;

public class Response
{
    public string StatusCode { get; set; }
    public string Message { get; set; }
    public object? Data { get; set; }
    public List<string> errors { get; }

    public Response(string statusCode, string message, object? data)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
        errors = new List<string>();
    }
}
