using AutoMapper;
using HotelServices.BLL.DTO;
using HotelServices.BLL.Infrastructure;
using HotelServices.BLL.Interfaces;
using HotelServices.DAL.Entities;
using HotelServices.DAL.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.Services
{
    /// <summary>
    /// Class for working with applications from the database using IUnitOfWork 
    /// </summary>
    public class ApplicationService: IApplication
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Constuctor which gets IUnitOfWork; with help of it we interact with the DAL level
        /// </summary>
        /// <param name="uow"></param>
        public ApplicationService(IUnitOfWork uow)
        {
            Database = uow;
        }

        /// <summary>
        /// Adding a new booking application to the database
        /// </summary>
        /// <param name="bookingAppDto"></param>
        public void SendApplication(BookingApplicationDTO bookingAppDto)
        {
            BookingApplication bookingApplication = new BookingApplication
            {
                Class = bookingAppDto.Class,
                Seats = bookingAppDto.Seats,
                CheckIn = bookingAppDto.CheckIn.Date,
                CheckOut = bookingAppDto.CheckOut.Date,
                UserEmail = bookingAppDto.UserEmail,
                UserId = bookingAppDto.UserId
            };
            Database.BookingApplications.Create(bookingApplication);
            Database.Save();
            logger.Log(LogLevel.Info, "Заявка №" + bookingApplication.Id + " на бронирование номера оформлена и " +
                "сохранена в базу данных.");
        }

        /// <summary>
        /// Getting a list of all booking applications from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BookingApplicationDTO> GetApplications()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<BookingApplication, BookingApplicationDTO>());
            logger.Log(LogLevel.Info, "Взяты все заявки на бронирование из базы данных.");
            return Mapper.Map<IEnumerable<BookingApplication>, List<BookingApplicationDTO>>(Database.BookingApplications.GetAll());
        }

        /// <summary>
        /// Getting a list of all messages from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MessageDTO> GetMessages()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Message, MessageDTO>());
            logger.Log(LogLevel.Info, "Взяты все сообщения из базы данных.");
            return Mapper.Map<IEnumerable<Message>, List<MessageDTO>>(Database.Messages.GetAll());
        }

        /// <summary>
        /// Getting an application from the database for further work with it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BookingApplicationDTO GetApplication(int? id)
        {
            if (id == null)
                throw new ValidationException("Не установлено id запроса", "");
            var app = Database.BookingApplications.Get(id.Value);
            if (app == null)
                throw new ValidationException("Запрос не найден", "");
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<BookingApplication, BookingApplicationDTO>());
            logger.Log(LogLevel.Info, "Из базы данных была взята заявка на бронирование №" + id + ".");
            return Mapper.Map<BookingApplication, BookingApplicationDTO>(app);
        }

        /// <summary>
        /// Adding a new booking and a message for user with confirmation to the database
        /// </summary>
        /// <param name="bookingDTO"></param>
        public void BookForManager(BookingDTO bookingDTO)
        {
            Room room = Database.Rooms.Get(bookingDTO.RoomId);
            BookingApplication bookingApplication = Database.BookingApplications.Get(bookingDTO.ApplicationId);
            if (room == null)
                throw new ValidationException("Комната не найдена", "");
            int days = Math.Abs((bookingDTO.CheckIn - bookingDTO.CheckOut).Days);
            decimal sum = room.Price * days;
            Booking booking = new Booking
            {
                Room = room,
                Sum = sum,
                IsPaid = false,
                CheckIn = bookingDTO.CheckIn.Date,
                CheckOut = bookingDTO.CheckOut.Date,
                UserEmail = bookingApplication.UserEmail,
                UserId = bookingApplication.UserId,
                Time = DateTime.Now
            };
            Database.Bookings.Create(booking);            
            Database.Save();
            Message confirmation = new Message
            {
                UserEmail = bookingApplication.UserEmail,
                DateTime = DateTime.Now,
                MessageText = "Уважаемый пользователь, на Вас было оформлено бронирование №" + booking.Id + ".Проверьте " +
                "его во вкладке «Мои бронирования». Бронирование должно быть оплачено в течении 48 часов! В противном случае " +
                "бронирование снимается."
            };
            Database.Messages.Create(confirmation);
            Database.BookingApplications.Delete(bookingApplication.Id);
            Database.Save();
            logger.Log(LogLevel.Info, "На пользователя " + booking.UserEmail + " в базу данных было занесено новое " +
                "бронирование №" + booking.Id + ".");
        }

        /// <summary>
        /// Adding a message for user with refuse to the database
        /// </summary>
        /// <param name="ApplicationId"></param>
        public void SendRefusal(int ApplicationId)
        {
            BookingApplication Application = Database.BookingApplications.Get(ApplicationId);
            Message negation = new Message
            {
                UserEmail = Application.UserEmail,
                DateTime = DateTime.Now,
                MessageText = "Уважаемый пользователь, к несчастью сообщаем, что по заявке №"+ ApplicationId +
                " в наличии нет свободных номеров по заданным критериям на заданные даты."
            };
            Database.Messages.Create(negation);
            Database.BookingApplications.Delete(ApplicationId);
            Database.Save();
            logger.Log(LogLevel.Info, "На пользователя " + negation.UserEmail + " в базу данных было занесено" +
                "сообщение с отказом на бронирование.");
        }

        /// <summary>
        /// Deleting an application from the database
        /// </summary>
        /// <param name="id"></param>
        public void DeleteApp(int id)
        {
            Database.BookingApplications.Delete(id);
            Database.Save();
            logger.Log(LogLevel.Info, "Заявка №" + id + " была удалена из базы данных.");
        }             
    }
}
