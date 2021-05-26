using BOL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.Helper
{
    public static class ResponseHandler
    {
        public static ResponseModel GetResponse(string Status, string Message, string ErrorCode, object Body, string AccessToken)
        {
            ResponseModel model = new ResponseModel
            {
                Status = Status,
                Message = Message,
                Code = ErrorCode,
                Body = Body,
                AccessToken = AccessToken
            };

            return model;
        }
    }
}
