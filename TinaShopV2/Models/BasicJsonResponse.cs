using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TinaShopV2.Models
{
    public class BasicJsonResponse
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public object Result { get; set; }
    }
}