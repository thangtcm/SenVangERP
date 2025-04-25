using System.Net;
using Application.Extentions;
using Shared.Enums;

namespace Application.Bases
{
    public class BaseResponse
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
        public HttpStatusCode StatusCode { get; set; }
        public BaseResponse()
        {
        }
        public BaseResponse(ResponseCodes responseCode, string message)
        {
            StatusCode = (HttpStatusCode)responseCode.GetStatusCode();
            Message = message;
        }

        public static BaseResponse Success(string? message = null)
        {
            return new BaseResponse()
            {
                Succeeded = true,
                Message = message ?? "Request was successful",
                StatusCode = HttpStatusCode.OK
            };
        }

        public static BaseResponse Failure(string? message = null)
        {
            return new BaseResponse()
            {
                Message = message,
                StatusCode = HttpStatusCode.BadRequest
            };

        }
    }
    public class BaseResponse<T> : BaseResponse
    {
        public object? Meta { get; set; }
        public T Data { get; set; }
        public BaseResponse()
        {

        }
        public BaseResponse(T data, string? message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public BaseResponse(string message)
        {
            Succeeded = false;
            Message = message;
        }
        public BaseResponse(string message, bool succeeded)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public BaseResponse(string message, bool succeeded, HttpStatusCode statusCode)
        {
            Succeeded = succeeded;
            Message = message;
            StatusCode = statusCode;
        }
    }
}
