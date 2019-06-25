using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelServices.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using HotelServices.DAL.Interfaces;
using HotelServices.DAL.Entities;

namespace HotelServices.BLL.Services.Tests
{
    /// <summary>
    /// Class for testing review service
    /// </summary>
    [TestClass()]
    public class ReviewServiceTests
    {
        /// <summary>
        /// Testing GetReviews method
        /// </summary>
        [TestMethod()]
        public void GetReviewsTest()
        {
            var mockRepository = new Mock<IRepository<Review>>();
            mockRepository.Setup(x => x.GetAll()).Returns(new List<Review> {
                new Review{ Id = 1 }
            }.AsEnumerable());
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.Reviews).Returns(mockRepository.Object);

            ReviewService review = new ReviewService(uowMock.Object);
            var result = review.GetReviews();

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Testing GetReview method
        /// </summary>
        [TestMethod()]
        public void GetReviewTest()
        {
            var mockRepository = new Mock<IRepository<Review>>();
            mockRepository.Setup(x => x.Get(1)).Returns(new Review { Id = 1 });
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.Reviews).Returns(mockRepository.Object);

            ReviewService review = new ReviewService(uowMock.Object);
            var result = review.GetReview(1);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Testing DeleteReview method
        /// </summary>
        [TestMethod()]
        public void DeleteReviewTest()
        {
            var mockRepository = new Mock<IRepository<Review>>();
            mockRepository.Setup(x => x.Delete(1));
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.Reviews).Returns(mockRepository.Object);

            ReviewService review = new ReviewService(uowMock.Object);
            review.DeleteReview(1);

            mockRepository.Verify(x => x.Delete(1), Times.Once());
        }
    }
}