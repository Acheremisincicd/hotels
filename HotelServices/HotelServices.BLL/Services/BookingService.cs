using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelServices.BLL.Interfaces;
using HotelServices.BLL.DTO;
using HotelServices.DAL.Interfaces;
using HotelServices.DAL.Entities;
using HotelServices.BLL.Infrastructure;
using AutoMapper;
using NLog;

namespace HotelServices.BLL.Services
{
    /// <summary>
    /// Class for working with bookings from the database using IUnitOfWork 
    /// </summary>
    public class BookingService: IBooking
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Constuctor which gets IUnitOfWork; with help of it we interact with the DAL level
        /// </summary>
        /// <param name="uow"></param>
        public BookingService(IUnitOfWork uow)
        {
            Database = uow;
        }

        /// <summary>
        /// Getting a list of all rooms from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoomDTO> GetRooms()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Room, RoomDTO>().MaxDepth(2));
            logger.Log(LogLevel.Info, "Взяты все комнаты из базы данных.");
            return Mapper.Map<IEnumerable<Room>, List<RoomDTO>>(Database.Rooms.GetAll());
        }

        /// <summary>
        /// Getting a room from the database for further work with it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RoomDTO GetRoom(int? id)
        {
            if (id == null)
                throw new ValidationException("Не установлено id номера", "");
            var room = Database.Rooms.Get(id.Value);
            if (room == null)
                throw new ValidationException("Комната не найдена", "");
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Room, RoomDTO>().MaxDepth(2));
            logger.Log(LogLevel.Info, "Взята комната №" + id + " из базы данных.");
            return Mapper.Map<Room, RoomDTO>(room);
        }

        /// <summary>
        /// Getting a booking from the database for further work with it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BookingDTO GetBooking(int? id)
        {
            if (id == null)
                throw new ValidationException("Не установлено id бронирования", "");
            var booking = Database.Bookings.Get(id.Value);
            if (booking == null)
                throw new ValidationException("Бронирование не найдено", "");
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Booking, BookingDTO>().MaxDepth(2));
            logger.Log(LogLevel.Info, "Взято бронирование №" + id + " из базы данных.");
            return Mapper.Map<Booking, BookingDTO>(booking);
        }

        /// <summary>
        /// Updating of the existing booking in the database
        /// </summary>
        /// <param name="bookingDTO"></param>
        public void UpdateBooking(BookingDTO bookingDTO)
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<BookingDTO, Booking>());
            Booking booking = Mapper.Map<BookingDTO, Booking>(bookingDTO);
            logger.Log(LogLevel.Info, "Бронирование №" + bookingDTO.Id + " было обновлено в базе данных.");
            Database.Bookings.Update(booking);
        }

        /// <summary>
        /// Adding a new booking to the database
        /// </summary>
        /// <param name="bookingDTO"></param>
        public void Book(BookingDTO bookingDTO)
        {
            Room room = Database.Rooms.Get(bookingDTO.RoomId);
            if (room == null)
                throw new ValidationException("Комната не найдена", "");
            int days = Math.Abs((bookingDTO.CheckIn - bookingDTO.CheckOut).Days);
            decimal sum = room.Price * days;
            Booking booking = new Booking
            {
                RoomId = room.Id,
                Sum = sum,
                IsPaid = false,
                CheckIn = bookingDTO.CheckIn.Date,
                CheckOut = bookingDTO.CheckOut.Date,
                UserEmail = bookingDTO.UserEmail,
                UserId = bookingDTO.UserId,
                Time = DateTime.Now
            };
            Database.Bookings.Create(booking);
            Database.Save();
            logger.Log(LogLevel.Info, "На пользователя " + booking.UserEmail + " в базу данных было занесено новое " +
                "бронирование №" + booking.Id + ".");
        }

        /// <summary>
        /// Deleting a booking from the database
        /// </summary>
        /// <param name="id"></param>
        public void DeleteBooking(int id)
        {
            Database.Bookings.Delete(id);
            Database.Save();
            logger.Log(LogLevel.Info, "Бронирование №" + id + " было удалено из базы данных.");
        }

        /// <summary>
        /// Getting a list of all bookings from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BookingDTO> GetBookings()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Booking, BookingDTO>().MaxDepth(2));
            logger.Log(LogLevel.Info, "Взяты все бронирования из базы данных.");
            return Mapper.Map<IEnumerable<Booking>, List<BookingDTO>>(Database.Bookings.GetAll());
        }

        /// <summary>
        /// Adding a message with information about canceling of a booking to the database
        /// </summary>
        /// <param name="UserEmail"></param>
        /// <param name="id"></param>
        public void MessageAboutCancel(string UserEmail, int id)
        {
            Message cancelMessage = new Message
            {
                UserEmail = UserEmail,
                DateTime = DateTime.Now,
                MessageText = "Уважаемый пользователь, по техническим причинам бронирование №" + id + " было отменено." +
                "Приносим извинение за неудобства. "
            };
            Database.Messages.Create(cancelMessage);
            Database.Save();
            logger.Log(LogLevel.Info, "В базу данных добавлено сообщение об отмене бронирования №" + id + " на имя "
                + UserEmail + ".");
        }

        /// <summary>
        /// Adding a message with information about canceling of a booking if it has been 
        /// already paid to the database
        /// </summary>
        /// <param name="UserEmail"></param>
        /// <param name="id"></param>
        public void MessageAboutCancelIfPaid(string UserEmail, int id)
        {
            Message cancelMessage = new Message
            {
                UserEmail = UserEmail,
                DateTime = DateTime.Now,
                MessageText = "Уважаемый пользователь, по техническим причинам бронирование №" + id + " было отменено." +
                "Деньги, заплаченные за бронирование, были вернуты Вам на счёт. Приносим извинение за неудобства. "
            };
            Database.Messages.Create(cancelMessage);
            Database.Save();
            logger.Log(LogLevel.Info, "В базу данных добавлено сообщение об отмене бронирования №" + id + " на имя "
                + UserEmail + ".");
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
