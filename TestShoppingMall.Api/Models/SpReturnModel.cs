using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestShoppingMall.Api.Models
{
    public class SpReturnModel
    {
        public SpReturnModel(string rtnCd, string rtnMsg)
        {
            ReturnCode = rtnCd;
            ReturnMessage = rtnMsg;
        }

        public string ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
    }
}
