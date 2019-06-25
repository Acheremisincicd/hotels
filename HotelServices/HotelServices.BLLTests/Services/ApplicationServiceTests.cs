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
    /// Class for testing application service
    /// </summary>
    [TestClass()]
    public class ApplicationServiceTests
    {
        /// <summary>
        /// Testing GetApplications method
        /// </summary>
        [TestMethod()]
        public void GetApplicationsTest()
        {
            var mockRepository = new Mock<IRepository<BookingApplication>>();
            mockRepository.Setup(x => x.GetAll()).Returns(new List<BookingApplication> {
                new BookingApplication{ Id = 1 }
            }.AsEnumerable());
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.BookingApplications).Returns(mockRepository.Object);

            ApplicationService app = new ApplicationService(uowMock.Object);
            var result = app.GetApplications();

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Testing GetApplication method
        /// </summary>
        [TestMethod()]
        public void GetApplicationTest()
        {
            var mockRepository = new Mock<IRepository<BookingApplication>>();
            mockRepository.Setup(x => x.Get(1)).Returns(new BookingApplication { Id = 1 });
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.BookingApplications).Returns(mockRepository.Object);

            ApplicationService app = new ApplicationService(uowMock.Object);
            var result = app.GetApplication(1);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Testing DeleteApp method
        /// </summary>
        [TestMethod()]
        public void DeleteAppTest()
        {
            var mockRepository = new Mock<IRepository<BookingApplication>>();
            mockRepository.Setup(x => x.Delete(1));
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.BookingApplications).Returns(mockRepository.Object);

            ApplicationService app = new ApplicationService(uowMock.Object);
            app.DeleteApp(1);

            mockRepository.Verify(x => x.Delete(1), Times.Once());
        }

        /// <summary>
        /// Testing GetMessages method
        /// </summary>
        [TestMethod()]
        public void GetMessagesTest()
        {
            var mockRepository = new Mock<IRepository<Message>>();
            mockRepository.Setup(x => x.GetAll()).Returns(new List<Message> {
                new Message{ Id = 1 }
            }.AsEnumerable());
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.Messages).Returns(mockRepository.Object);

            ApplicationService app = new ApplicationService(uowMock.Object);
            var result = app.GetMessages();

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Testing SendRefusal method
        /// </summary>
        [TestMethod()]
        public void SendRefusalTest()
        {
            var mockRepositoryApp = new Mock<IRepository<BookingApplication>>();
            mockRepositoryApp.Setup(x => x.Get(1)).Returns(new BookingApplication { Id = 1 });
            var mockRepositoryMes = new Mock<IRepository<Message>>();
            mockRepositoryMes.Setup(x => x.Create(new Message { Id = 1 }));
            mockRepositoryApp.Setup(x => x.Delete(1));

            Assert.IsNotNull(mockRepositoryMes);
        }
    }
}