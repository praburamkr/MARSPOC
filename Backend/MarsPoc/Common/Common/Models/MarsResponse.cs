using System;
using System.Net;

namespace Common.Models
{
    public class MarsResponse
    {
        public HttpStatusCode Code { get; set; }

        public object Data { get; set; }

        public MarsException Error { get; set; }

        public MarsResponse()
        {
        }

        public static MarsResponse GetResponse(object data, HttpStatusCode code = HttpStatusCode.OK)
        {
            MarsResponse response = new MarsResponse();
            response.Code = code;
            response.Data = data;

            return response;
        }

        public static MarsResponse GetError(Exception ex)
        {
            MarsResponse response = new MarsResponse();
            response.Code = HttpStatusCode.InternalServerError;
            response.Error = new MarsException(ex.InnerException == null ? ex.Message : ex.InnerException.Message);

            return response;
        }
    }
}
