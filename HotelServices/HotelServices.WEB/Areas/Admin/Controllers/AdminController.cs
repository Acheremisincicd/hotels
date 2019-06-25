using AutoMapper;
using HotelServices.BLL.DTO;
using HotelServices.BLL.Interfaces;
using HotelServices.BLL.Services;
using HotelServices.WEB.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelServices.WEB.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for administrator
    /// </summary>
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        IBooking booking;

        /// <summary>
        /// IUserSerice object
        /// </summary>
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        /// <summary>
        /// Constructor with one parameter
        /// </summary>
        /// <param name="b"></param>
        public AdminController(IBooking b)
        {
            booking = b;
        }

        /// <summary>
        /// Displays a page with administrator actions
        /// </summary>
        /// <returns></returns>
        public ActionResult Actions()
        {
            return View();
        }

        /// <summary>
        /// Displays all of the booking from the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Bookings()
        {
            IEnumerable<BookingDTO> bookingDtos = booking.GetBookings();
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<BookingDTO, BookingViewModel>().MaxDepth(2));
            var bookingsVM = Mapper.Map<IEnumerable<BookingDTO>, List<BookingViewModel>>(bookingDtos);
            return View(bookingsVM);
        }

        /// <summary>
        /// Gets booking id and displays a page where administrator should choose whether he likes
        /// to cancel it or not
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConfirmationOfCancel(int id)
        {
            BookingDTO bookingDTO = booking.GetBooking(id);
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<BookingDTO, BookingViewModel>());
            var bookingVM = Mapper.Map<BookingDTO, BookingViewModel>(bookingDTO);
            if (bookingDTO.IsPaid == true)
            {
                string UserID = bookingDTO.UserId;
                UserDTO userDTO = UserService.GetUser(UserID);
                ViewBag.Address = userDTO.Address;
                ViewBag.ClientName = userDTO.ClientName;
                ViewBag.Email = userDTO.Email;
                ViewBag.Password = userDTO.Password;
                ViewBag.Role = userDTO.Role;
                ViewBag.UserName = userDTO.UserName;
                ViewBag.СashAccount = userDTO.СashAccount;
            }
            return View(bookingVM);
        }

        /// <summary>
        /// Gets booking id and user view model and deletes a booking from the database
        /// and updates user's cash account in the database 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Cancel(int id, UserViewModel userVM)
        {            
            BookingDTO bookingDTO = booking.GetBooking(id);
            if (bookingDTO.IsPaid == true)
                booking.MessageAboutCancelIfPaid(bookingDTO.UserEmail, bookingDTO.Id);
            else
                booking.MessageAboutCancel(bookingDTO.UserEmail, bookingDTO.Id);
            booking.DeleteBooking(id);
            if (bookingDTO.IsPaid == true)
            {
                userVM.Id = bookingDTO.UserId;
                userVM.СashAccount = userVM.СashAccount + bookingDTO.Sum;
                Mapper.Reset();
                Mapper.Initialize(cfg => cfg.CreateMap<UserViewModel, UserDTO>());
                var userDTO = Mapper.Map<UserViewModel, UserDTO>(userVM);
                UserService.UpdateClient(userDTO);
            }
            return RedirectToAction("Bookings");
        }
    }
}