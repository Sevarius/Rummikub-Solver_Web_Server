using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace EmptyMVC.Models
{
    public sealed class JsonNetResult<T> : ActionResult where T : class
    {
        private readonly T _data;
        public JsonNetResult(T data)
        {
            _data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof (context));

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";

            if (_data is null)
            {
                return;
            }

            response.Write(JsonConvert.SerializeObject(_data));
        }
    }
}