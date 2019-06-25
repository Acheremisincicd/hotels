using AutoMapper;
using HotelServices.BLL.DTO;
using HotelServices.BLL.Infrastructure;
using HotelServices.BLL.Interfaces;
using HotelServices.WEB.Filters;
using HotelServices.WEB.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HotelServices.WEB.Controllers
{
    /// <summary>
    /// Controller managing user data
    /// </summary>
    public class AccountController : Controller
    {
        IApplication application;
        IBooking booking;

        /// <summary>
        /// Constructor with 2 parametres
        /// </summary>
        /// <param name="app"></param>
        /// <param name="b"></param>
        public AccountController(IApplication app, IBooking b)
        {
            application = app;
            booking = b;
        }

        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [ExceptionFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверная почта или пароль.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Main", "Home");
                }
            }
            return View(model);
        }

        [ExceptionFilter]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Main", "Home");
        }

        [ExceptionFilter]
        [Authorize(Roles = "user, manager, admin")]
        public ActionResult PersonalAccount()
        {
            string id = User.Identity.GetUserId();
            UserDTO userDTO = UserService.GetUser(id);
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            var user = Mapper.Map<UserDTO, UserViewModel>(userDTO);
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }
            return View(user);
        }

        [ExceptionFilter]
        [Authorize(Roles = "user, manager, admin")]
        [HttpGet]
        public ActionResult Deposit()
        {
            string id = User.Identity.GetUserId();
            UserDTO userDTO = UserService.GetUser(id);
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            var user = Mapper.Map<UserDTO, UserViewModel>(userDTO);
            return View(user);
        }

        [ExceptionFilter]
        [Authorize(Roles = "user, manager, admin")]
        [HttpPost]
        public ActionResult Deposit(UserViewModel user, string oldValue)
        {
            decimal old = Convert.ToDecimal(oldValue);
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<UserViewModel, UserDTO>());
            var userDTO = Mapper.Map<UserViewModel, UserDTO>(user);
            userDTO.СashAccount = userDTO.СashAccount + old;
            UserService.UpdateClient(userDTO);
            return RedirectToAction("PersonalAccount");
        }

        [ExceptionFilter]
        [Authorize(Roles = "user, manager, admin")]
        public ActionResult MyMessageList()
        {
            IEnumerable<MessageDTO> messagesDTO = application.GetMessages();
            var messagesNeed = from m in messagesDTO
                               where m.UserEmail == User.Identity.Name
                               select m;
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<MessageDTO, MessageViewModel>());
            var messages = Mapper.Map<IEnumerable<MessageDTO>, List<MessageViewModel>>(messagesNeed);           
            return View(messages);
        }

        [ExceptionFilter]
        [Authorize(Roles = "user, manager, admin")]
        public ActionResult MyBookingList()
        {
            IEnumerable<BookingDTO> bookingsDTO = booking.GetBookings();
            var myBookings = from b in bookingsDTO
                             where b.UserEmail == User.Identity.Name
                             select b;
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<BookingDTO, BookingViewModel>().MaxDepth(2));
            var bookings = Mapper.Map<IEnumerable<BookingDTO>, List<BookingViewModel>>(myBookings);
            return View(bookings);
        }

        [ExceptionFilter]
        [HttpGet]
        [Authorize(Roles = "user, manager, admin")]
        public ActionResult PaymentСonfirmation(int? id)
        {
            BookingDTO bookingForPayDTO = booking.GetBooking(id);
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<BookingDTO, BookingViewModel>());
            var bookingForPay = Mapper.Map<BookingDTO, BookingViewModel>(bookingForPayDTO);
            string UserId = User.Identity.GetUserId();
            UserDTO userDTO = UserService.GetUser(UserId);
            ViewBag.Address = userDTO.Address;
            ViewBag.ClientName = userDTO.ClientName;
            ViewBag.Email = userDTO.Email;
            ViewBag.Password = userDTO.Password;
            ViewBag.Role = userDTO.Role;
            ViewBag.UserName = userDTO.UserName;
            ViewBag.СashAccount = userDTO.СashAccount;
            return View(bookingForPay);
        }

        [ExceptionFilter]
        [HttpPost]
        public ActionResult PaymentСonfirmation(BookingViewModel bookingVM, UserViewModel userVM)
        {
            userVM.Id = User.Identity.GetUserId();
            bookingVM.IsPaid = true;
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<BookingViewModel, BookingDTO>());
            var bookingDTO = Mapper.Map<BookingViewModel, BookingDTO>(bookingVM);
            booking.UpdateBooking(bookingDTO);
            userVM.СashAccount = userVM.СashAccount - bookingVM.Sum;
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<UserViewModel, UserDTO>());
            var userDTO = Mapper.Map<UserViewModel, UserDTO>(userVM);
            UserService.UpdateClient(userDTO);
            return View("Pay");
        }     

        //exception
        [ExceptionFilter]
        [Authorize(Roles = "user, manager, admin")]
        public ActionResult Pay(int? id)
        {
            BookingDTO bookingForPay = booking.GetBooking(id);
            string UserId = User.Identity.GetUserId();
            UserDTO userDTO = UserService.GetUser(UserId);
            if (userDTO.СashAccount < bookingForPay.Sum)
            {
                return View("ErrorPaying");
            }
            bookingForPay.IsPaid = true;
            booking.UpdateBooking(bookingForPay);
            userDTO.СashAccount = userDTO.СashAccount - bookingForPay.Sum;
            UserService.UpdateClient(userDTO);
            return View();
        }

        [ExceptionFilter]
        [Authorize(Roles = "user, manager, admin")]
        public ActionResult MyApplicationList()
        {
            IEnumerable<BookingApplicationDTO> applicationsDTO = application.GetApplications();
            var myApplications = from a in applicationsDTO
                             where a.UserEmail == User.Identity.Name
                             select a;
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<BookingApplicationDTO, BookingApplicationViewModel>());
            var applications = Mapper.Map<IEnumerable<BookingApplicationDTO>, List<BookingApplicationViewModel>>(myApplications);
            return View(applications);
        }

        [ExceptionFilter]
        public ActionResult Cancel(int id)
        {
            application.DeleteApp(id);
            return RedirectToAction("MyApplicationList");
        }

        [ExceptionFilter]
        [Authorize(Roles = "user, manager, admin")]
        public ActionResult Refuse(int id, string UserEmail)
        {
            booking.DeleteBooking(id);
            return RedirectToAction("MyBookingList");
        }

        public ActionResult Register()
        {
            return View();
        }

        [ExceptionFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Address = model.Address,
                    ClientName = model.ClientName,
                    СashAccount = 100.0m,
                    Role = "user"
                };
                OperationDetails operationDetails = await UserService.Create(userDto);
                if (operationDetails.Succedeed)
                    return View("SuccessRegister");
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        [Authorize(Roles = "user, manager, admin")]
        [HttpGet]
        public ActionResult SuccessRegister()
        {
            return View();
        }

        private async Task SetInitialDataAsync()
        {
            await UserService.SetInitialData(new UserDTO
            {
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com",
                Password = "123Aa_",
                ClientName = "Vitaliy",
                Address = "Kharkiv",
                Role = "admin",
            },
            new UserDTO
            {
                Email = "manager@gmail.com",
                UserName = "manager@gmail.com",
                Password = "123Aa_",
                ClientName = "Vitaliy",
                Address = "Kharkiv",
                Role = "manager",
            },
            new List<string> { "user", "admin", "manager"});
        }
    }
}