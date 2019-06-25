using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelServices.WEB.Controllers
{
    /// <summary>
    /// Information controller
    /// </summary>
    public class InfoController : Controller
    {
        /// <summary>
        /// Displays a page with hotel rules
        /// </summary>
        /// <returns></returns>
        public ActionResult Rules()
        {
            return View();
        }

        /// <summary>
        /// Displays a page with hotel contacts
        /// </summary>
        /// <returns></returns>
        public ActionResult Contacts()
        {
            return View();
        }

        /// <summary>
        /// Displays a page with hotel gallery
        /// </summary>
        /// <returns></returns>
        public ActionResult Gallery()
        {
            return View();
        }
    }
}