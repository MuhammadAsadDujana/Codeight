using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.Common
{
    public class ResponseModel
    {
        public string Status { get; set; }

        public string Code { get; set; }

        public object Body { get; set; }

        public string Message { get; set; }

        public string AccessToken { get; set; }
    }
}
