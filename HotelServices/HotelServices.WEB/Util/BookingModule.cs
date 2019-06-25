using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HotelServices.BLL.Interfaces;
using HotelServices.BLL.Services;

namespace HotelServices.WEB.Util
{
    /// <summary>
    /// Class with dependency resolve
    /// </summary>
    public class BookingModule: NinjectModule
    {
        /// <summary>
        /// Overriding method Load, which resolves dependency between
        /// the classes
        /// </summary>
        public override void Load()
        {
            Bind<IBooking>().To<BookingService>();
            Bind<IApplication>().To<ApplicationService>();
            Bind<IReview>().To<ReviewService>();
        }
    }
}