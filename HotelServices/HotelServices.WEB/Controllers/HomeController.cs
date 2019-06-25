using AutoMapper;
using HotelServices.BLL.DTO;
using HotelServices.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelServices.WEB.Models;
using HotelServices.BLL.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using HotelServices.WEB.Filters;

namespace HotelServices.WEB.Controllers
{
    /// <summary>
    /// Home controller
    /// </summary>
    public class HomeController : Controller
    {
        IBooking booking;

        /// <summary>
        /// Constructor with one parameter
        /// </summary>
        /// <param name="b"></param>
        public HomeController(IBooking b)
        {
            booking = b;
        }

        /// <summary>
        /// Main page; here is checking how many days have passed after booking and delets
        /// it if it more than 2 days of 48 hours
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Main()
        {
            IEnumerable<BookingDTO> bookingDTO = booking.GetBookings();
            foreach (var b in bookingDTO)
            {
                int daysBetween = (DateTime.Now - b.Time).Hours;
                if (daysBetween > 48 && b.IsPaid==false)
                {
                    booking.DeleteBooking(b.Id);
                }
            }
            return View();
        }

        /// <summary>
        /// Displays all of the room from the database
        /// </summary>
        /// <returns></returns>
        [ExceptionFilter]
        [HttpGet]
        public ActionResult Rooms()
        {
            IEnumerable<RoomDTO> roomsDtos = booking.GetRooms();
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<RoomDTO, RoomViewModel>());
            var rooms = Mapper.Map<IEnumerable<RoomDTO>, List<RoomViewModel>>(roomsDtos);
            return View(rooms);
        }

        /// <summary>
        /// Gets room id and offers to enter dates for booking
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ExceptionFilter]
        [Authorize(Roles = "user, manager, admin")]
        [HttpGet]
        public ActionResult ToBook(int? id)
        {
            RoomDTO roomDto = booking.GetRoom(id);
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<RoomDTO, BookingViewModel>()
                    .ForMember("RoomId", opt => opt.MapFrom(src => src.Id)));
            var bookingVM = Mapper.Map<RoomDTO, BookingViewModel>(roomDto);
            bookingVM.UserEmail = User.Identity.Name;
            return View(bookingVM);
        }

        /// <summary>
        /// Analyzes the received data and if it's no booking of the room on recieved 
        /// date, creates a new booking
        /// </summary>
        /// <param name="bookingVM"></param>
        /// <returns></returns>
        [ExceptionFilter]
        [Authorize(Roles = "user, manager, admin")]
        [HttpPost]
        public ActionResult ToBook(BookingViewModel bookingVM)
        {
            bookingVM.UserId = User.Identity.GetUserId();
            if (bookingVM.CheckIn >= bookingVM.CheckOut)
            {
                ModelState.AddModelError("", "Введите корректный интервал дат!");
                return View(bookingVM);
            }
            
            IEnumerable<BookingDTO> bookingDTO = booking.GetBookings();
            var bookings = from b in bookingDTO
                           where b.RoomId == bookingVM.RoomId
                           select b;
            foreach(var b in bookings)
            {
                if (bookingVM.CheckOut <= b.CheckIn || bookingVM.CheckIn >= b.CheckOut)
                { }
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
                booking.Book(bookingDto);
                return View("BookedSuccessfully");
            }
            return View(bookingVM);
        }

        /// <summary>
        /// Page with information about successful booking
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "user, manager, admin")]
        [HttpGet]
        public ActionResult BookedSuccessfully()
        {
            return View();
        }

        /// <summary>
        /// Displays all of the booking from the database
        /// </summary>
        /// <returns></returns>
        [ExceptionFilter]
        [Authorize(Roles = "manager, admin")]
        [HttpGet]
        public ActionResult ShowBookings()
        {
            IEnumerable<BookingDTO> bookingDTO = booking.GetBookings();
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<BookingDTO, BookingViewModel>().MaxDepth(2));
            var bookings = Mapper.Map<IEnumerable<BookingDTO>, List<BookingViewModel>>(bookingDTO);
            return View(bookings);
        }

        /// <summary>
        /// Sorts rooms according to selected criteria and displays them on the page
        /// </summary>
        /// <returns></returns>
        [ExceptionFilter]
        public ActionResult Sorting()
        {
            string criterion = Request.Form["criterion"];
            string way = Request.Form["way"];
            ViewBag.Criterion = criterion;
            ViewBag.Way = way;
            IEnumerable<RoomDTO> roomsDtos = booking.GetRooms();
            List<RoomDTO> sortedRoomsDTO = roomsDtos.OrderBy(r => r.Class).ToList();
            switch (criterion)
            {
                case "Id":
                    if (way == "up")
                        sortedRoomsDTO = roomsDtos.OrderBy(r => r.Id).ToList();
                    if (way == "down")
                        sortedRoomsDTO = roomsDtos.OrderByDescending(r => r.Id).ToList();
                    break;
                case "Class":
                    if (way == "up")
                        sortedRoomsDTO = roomsDtos.OrderBy(r => r.Class).ToList();
                    if (way == "down")
                        sortedRoomsDTO = roomsDtos.OrderByDescending(r => r.Class).ToList();
                    break;
                case "Seats":
                    if (way == "up")
                        sortedRoomsDTO = roomsDtos.OrderBy(r => r.Seats).ToList();
                    if (way == "down")
                        sortedRoomsDTO = roomsDtos.OrderByDescending(r => r.Seats).ToList();
                    break;
                case "Price":
                    if (way == "up")
                        sortedRoomsDTO = roomsDtos.OrderBy(r => r.Price).ToList();
                    if (way == "down")
                        sortedRoomsDTO = roomsDtos.OrderByDescending(r => r.Price).ToList();
                    break;
                case "Status":
                    if (way == "up")
                        sortedRoomsDTO = roomsDtos.OrderBy(r => r.Status).ToList();
                    if (way == "down")
                        sortedRoomsDTO = roomsDtos.OrderByDescending(r => r.Status).ToList();
                    break;
            }
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<RoomDTO, RoomViewModel>());
            var sortedRooms = Mapper.Map<List<RoomDTO>, List<RoomViewModel>>(sortedRoomsDTO);           
            return View(sortedRooms);
        }

        protected override void Dispose(bool disposing)
        {
            booking.Dispose();
            base.Dispose(disposing);
        }
    }
}