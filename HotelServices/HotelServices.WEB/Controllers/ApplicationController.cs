using AutoMapper;
using HotelServices.BLL.DTO;
using HotelServices.BLL.Infrastructure;
using HotelServices.BLL.Interfaces;
using HotelServices.WEB.Filters;
using HotelServices.WEB.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelServices.WEB.Controllers
{
    /// <summary>
    /// Booking application controller
    /// </summary>
    public class ApplicationController : Controller
    {
        IApplication application;
        IBooking booking;

        /// <summary>
        /// Constructor with 2 parametres
        /// </summary>
        /// <param name="app"></param>
        /// <param name="b"></param>
        public ApplicationController (IApplication app, IBooking b)
        {
            application = app;
            booking = b;
        }

        /// <summary>
        /// Displays the page where user choose criterions of the room he wants
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "user, manager, admin")]
        [HttpGet]
        public ActionResult LeaveApplication()
        {
            return View();
        }

        /// <summary>
        /// Gets booking application view model, checks it and if everything is correct,
        /// adds it to the database
        /// </summary>
        /// <param name="bookingApplication"></param>
        /// <returns></returns>
        [ExceptionFilter]
        [Authorize(Roles = "user, manager, admin")]
        [HttpPost]
        public ActionResult LeaveApplication(BookingApplicationViewModel bookingApplication)
        {
            if (bookingApplication.CheckIn >= bookingApplication.CheckOut)
            {
                ModelState.AddModelError("", "Введите корректный интервал дат!");
                return View(bookingApplication);
            }
            if (ModelState.IsValid)
            {
                Mapper.Reset();
                Mapper.Initialize(cfg => cfg.CreateMap<BookingApplicationViewModel, BookingApplicationDTO>());
                var bookingApplicationDto = Mapper.Map<BookingApplicationViewModel, BookingApplicationDTO>(bookingApplication);
                bookingApplicationDto.UserEmail = bookingApplication.UserEmail;
                bookingApplicationDto.UserId = User.Identity.GetUserId();
                application.SendApplication(bookingApplicationDto);
                return View("ApplicationSubmitted", bookingApplication);             
            }
            return View();
        }

        /// <summary>
        /// Displays the information about successful submit of the application
        /// </summary>
        /// <param name="bookingApplication"></param>
        /// <returns></returns>
        [ExceptionFilter]
        [Authorize(Roles = "user, manager, admin")]
        [HttpGet]
        public ActionResult ApplicationSubmitted(BookingApplicationViewModel bookingApplication)
        {
            return View();
        }

        /// <summary>
        /// Displays all of the application from the database
        /// </summary>
        /// <returns></returns>
        [ExceptionFilter]
        [Authorize(Roles = "manager, admin")]
        [HttpGet]
        public ActionResult ShowApplications()
        {
            IEnumerable<BookingApplicationDTO> bookingApplicationDTO = application.GetApplications();
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<BookingApplicationDTO, BookingApplicationViewModel>());
            var applications = Mapper.Map<IEnumerable<BookingApplicationDTO>, List<BookingApplicationViewModel>>(bookingApplicationDTO);
            return View(applications);
        }

        /// <summary>
        /// Displays all of the room from the database when manager/administrator is trying
        /// to make a new application in the database
        /// </summary>
        /// <param name="ApplicationId"></param>
        /// <returns></returns>
        [ExceptionFilter]
        [Authorize(Roles = "manager, admin")]
        [HttpGet]
        public ActionResult RoomsForManager(int? ApplicationId)
        {
            IEnumerable<RoomDTO> roomDtos = booking.GetRooms();
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<RoomDTO, RoomViewModel>());
            var rooms = Mapper.Map<IEnumerable<RoomDTO>, List<RoomViewModel>>(roomDtos);
            ViewBag.ApplicationId = ApplicationId;
            return View(rooms);
        }

        /// <summary>
        /// Gets room id and offers to enter dates for booking
        /// </summary>
        /// <param name="RoomId"></param>
        /// <param name="ApplicationId"></param>
        /// <returns></returns>
        [ExceptionFilter]
        [Authorize(Roles = "manager, admin")]
        [HttpGet]
        public ActionResult ToBookForManager(int? RoomId, int? ApplicationId)
        {
            RoomDTO roomDto = booking.GetRoom(RoomId);
            BookingApplicationDTO applicationDTO = application.GetApplication(ApplicationId);
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<RoomDTO, BookingViewModel>()
                    .ForMember("RoomId", opt => opt.MapFrom(src => src.Id)));
            var bookingVM = Mapper.Map<RoomDTO, BookingViewModel>(roomDto);
            bookingVM.ApplicationId = ApplicationId;
            return View(bookingVM);
        }

        /// <summary>
        /// Analyzes the received data and if it's no booking of the room on recieved 
        /// date, creates a new booking
        /// </summary>
        /// <param name="bookingVM"></param>
        /// <returns></returns>
        [ExceptionFilter]
        [Authorize(Roles = "manager, admin")]
        [HttpPost]
        public ActionResult ToBookForManager(BookingViewModel bookingVM)
        {
            if (bookingVM.CheckIn >= bookingVM.CheckOut)
            {
                ModelState.AddModelError("", "Введите корректный интервал дат!");
                return View(bookingVM);
            }

            IEnumerable<BookingDTO> bookingDTO = booking.GetBookings();
            var bookings = from b in bookingDTO
                           where b.RoomId == bookingVM.RoomId
                           select b;
            foreach (var b in bookings)
            {
                if (bookingVM.CheckOut <= b.CheckIn || bookingVM.CheckIn >= b.CheckOut)
                {

                }
                else
                {
                    ModelState.AddModelError("", "Данная комната занята на эти даты!");
                    return View(bookingVM);
                }
            }

            if (ModelState.IsValid)
            {
                Mapper.Reset();
                Mapper.Initialize(cfg => cfg.CreateMap<BookingViewModel, BookingDTO>());
                var bookingDto = Mapper.Map<BookingViewModel, BookingDTO>(bookingVM);
                application.BookForManager(bookingDto);
                return View("BookedSuccessfully");
            }
            return View(bookingVM);
        }

        /// <summary>
        /// Page with information about successful booking
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "manager, admin")]
        [HttpGet]
        public ActionResult BookedSuccessfully()
        {
            return View();
        }

        /// <summary>
        /// Adds a new message with information about refuse to the database
        /// </summary>
        /// <param name="ApplicationId"></param>
        /// <returns></returns>
        [ExceptionFilter]
        [Authorize(Roles = "manager, admin")]
        public ActionResult SendRefusal(int ApplicationId)
        {
            application.SendRefusal(ApplicationId);           
            return View();
        }
    }
}