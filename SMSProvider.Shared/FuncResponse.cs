using System.Net;

namespace SMSProvider.Shared;

public class FuncResponse
{
    public FuncResponse(HttpStatusCode statusCode = HttpStatusCode.OK, ResponseCode responseCode = ResponseCode.Success, string message = "")
    {
        ResponseCode = responseCode;
        Message = message;
        HttpStatusCode = statusCode;
    }

    public string Message { get; set; }
    public ResponseCode ResponseCode { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }
    public bool IsSuccess => ResponseCode == ResponseCode.Success;
}

public class FuncResponseWithValue<T> : FuncResponse
{
    public FuncResponseWithValue(T data, HttpStatusCode statusCode = HttpStatusCode.OK, ResponseCode responseCode = ResponseCode.Success, string message = "")
        : base(statusCode, responseCode, message)
    {
        Data = data;
    }

    public T Data { get; set; }
}