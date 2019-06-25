using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelServices.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelServices.DAL.Entities;
using Moq;
using HotelServices.DAL.Interfaces;
using HotelServices.BLL.DTO;
using System.Data.Entity;
using HotelServices.DAL.EF;

namespace HotelServices.BLL.Services.Tests
{
    /// <summary>
    /// Class for testing booking service
    /// </summary>
    [TestClass()]
    public class BookingServiceTests
    {
        /// <summary>
        /// Testing GetRooms method
        /// </summary>
        [TestMethod()]
        public void GetRoomsTest()
        {
            var mockRepository = new Mock<IRepository<Room>>();
            mockRepository.Setup(x => x.GetAll()).Returns(new List<Room> {
                new Room{ Id = 1 }
            }.AsEnumerable());
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.Rooms).Returns(mockRepository.Object);

            BookingService booking = new BookingService(uowMock.Object);
            var result = booking.GetRooms();

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Testing GetRoom method
        /// </summary>
        [TestMethod()]
        public void GetRoomTest()
        {
            var mockRepository = new Mock<IRepository<Room>>();
            mockRepository.Setup(x => x.Get(1)).Returns(new Room { Id = 1 });
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.Rooms).Returns(mockRepository.Object);

            BookingService booking = new BookingService(uowMock.Object);
            var result = booking.GetRoom(1);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Testing GetBooking method
        /// </summary>
        [TestMethod()]
        public void GetBookingTest()
        {
            var mockRepository = new Mock<IRepository<Booking>>();
            mockRepository.Setup(x => x.Get(1)).Returns(new Booking { Id = 1 });
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.Bookings).Returns(mockRepository.Object);

            BookingService booking = new BookingService(uowMock.Object);
            var result = booking.GetBooking(1);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Testing GetBookings method
        /// </summary>
        [TestMethod()]
        public void GetBookingsTest()
        {
            var mockRepository = new Mock<IRepository<Booking>>();
            mockRepository.Setup(x => x.GetAll()).Returns(new List<Booking> {
                new Booking{ Id = 1 }
            }.AsEnumerable());
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.Bookings).Returns(mockRepository.Object);

            BookingService booking = new BookingService(uowMock.Object);
            var result = booking.GetBookings();

            Assert.IsNotNull(result);
        }

        //[TestMethod()]
        //public void UpdateBookingTest()
        //{
        //    var mockRepository = new Mock<IRepository<Booking>>();
        //    mockRepository.Setup(x => x.Update(new Booking { Id = 1 }));
        //    var uowMock = new Mock<IUnitOfWork>();
        //    uowMock.Setup(x => x.Bookings).Returns(mockRepository.Object);

        //    BookingService booking = new BookingService(uowMock.Object);
        //    booking.UpdateBooking(new BookingDTO { Id = 1 });

        //    mockRepository.Verify(x => x.Update(new Booking { Id = 1 }));
        //}

        /// <summary>
        /// Testing DeleteBooking method
        /// </summary>
        [TestMethod()]
        public void DeleteBookingTest()
        {
            var mockRepository = new Mock<IRepository<Booking>>();
            mockRepository.Setup(x => x.Delete(1));
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.Bookings).Returns(mockRepository.Object);

            BookingService booking = new BookingService(uowMock.Object);
            booking.DeleteBooking(1);

            mockRepository.Verify(x => x.Delete(1), Times.Once());
        }
    }
}