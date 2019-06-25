using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace HotelServices.WEB.Filters
{
    /// <summary>
    /// My own exception handler
    /// </summary>
    public class ExceptionFilterAttribute : HandleErrorAttribute
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Overriding OnException method
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];
            Exception innerException = filterContext.Exception;
            if ((new HttpException(null, innerException).GetHttpCode() == 500) 
                && this.ExceptionType.IsInstanceOfType(innerException))
            {
                var viewData = new ViewDataDictionary<HandleErrorInfo>(filterContext.Controller.ViewData);
                viewData.Model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
                filterContext.Result = new ViewResult {
                    ViewName = this.View,
                    MasterName = this.Master,
                    ViewData = viewData,
                    TempData = filterContext.Controller.TempData };
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }

            //creating a message with exception information, which will be passed to error-log file
            StringBuilder errorMsg = new StringBuilder("Action: [");
            errorMsg.Append(actionName);
            errorMsg.Append("], Controller: [");
            errorMsg.Append(controllerName);
            errorMsg.Append("], Type: [");
            errorMsg.Append(filterContext.Exception.GetType());
            errorMsg.Append("], Message: [");
            errorMsg.Append(filterContext.Exception.Message);
            errorMsg.Append("] ");
            logger.Log(LogLevel.Error, errorMsg);
        }
    }
}