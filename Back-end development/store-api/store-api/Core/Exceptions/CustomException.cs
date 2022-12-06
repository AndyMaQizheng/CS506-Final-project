using store_api.Core.DTOs.Responses;
using System;

namespace store_api.Core.Exceptions
{
    public sealed class CustomException : Exception
    {
        private string Code;

        public CustomException(string message, string code = "02") : base(message)
        {
            Code = code;
        }

        public FailureResponse Get()
        {
            return new FailureResponse(Code, Message);
        }

    }
}
