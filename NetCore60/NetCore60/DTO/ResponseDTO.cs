using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace NetCore60.DTO
{
    public class ResponseDTO
    {
        public string Status { get; set; }
        public object Result { get; set; }
        public bool Success { get; set; }
        public static ResponseDTO SuccessResponse(object result = null)
        {
            return new ResponseDTO
            {
                Status = "OK",
                Success = true,
                Result = result
            };
        }
        public static ResponseDTO ErrorResponse(string errorCode, object result = null)
        {
            return new ResponseDTO
            {
                Status = errorCode,
                Success = false,
                Result = result
            };
        }
    }
}