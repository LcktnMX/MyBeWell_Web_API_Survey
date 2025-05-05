using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lockton.Surveys.Web.Attributes
{
    public class AllowedExtensionsAttribute : ActionFilterAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            foreach (var file in context.HttpContext.Request.Form.Files) {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    context.Result = new BadRequestObjectResult( $"Extension de archivo invalida");
                }
            }

        }
    }
}
