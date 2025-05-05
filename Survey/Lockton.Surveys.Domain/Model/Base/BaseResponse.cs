using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.Domain.Model.Base
{
   public class BaseResponse
    {
        public BaseResponse()
        {
            this.Success = true;
            this.Message = string.Empty;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
