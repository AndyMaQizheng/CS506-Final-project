using store_api.Core.Interfaces;

namespace store_api.Core.DTOs.Responses
{
    public class Response
    {
        public string Status { get; set; } = "success";
        public string StatusCode { get; set; } = "00";
        public string Message { get; set; }
    }

    public class BaseResponse : IResponse
    {
        public string Status { get; set; } = "success";
        public string StatusCode { get; set; } = "00";
        public string Message { get; set; }
    }

    public class SuccessResponse : BaseResponse
    {
        public dynamic Data { get; set; }
        public SuccessResponse(string message, dynamic data = null, string statuscode = "00", string status = "success")
        {
            Status = status;
            StatusCode = statuscode;
            Message = message;
            Data = data;
        }
    }

    public class FailureResponse : BaseResponse
    {
        public FailureResponse(string statusCode, string message)
        {
            Status = "failed";
            StatusCode = statusCode;
            Message = message;
        }
    }

    public class ResponseWithData<T> where T : new()
    {
        public string Status { get; set; } = "success";
        public string StatusCode { get; set; } = "00";
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
