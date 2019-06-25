using AutoMapper;
using HotelServices.BLL.DTO;
using HotelServices.BLL.Infrastructure;
using HotelServices.BLL.Interfaces;
using HotelServices.DAL.Entities;
using HotelServices.DAL.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.Services
{
    /// <summary>
    /// Class for working with reviews from database using IUnitOfWork 
    /// </summary>
    public class ReviewService: IReview
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Constuctor which gets IUnitOfWork; with help of it we interact with the DAL level
        /// </summary>
        /// <param name="uow"></param>
        public ReviewService(IUnitOfWork uow)
        {
            Database = uow;
        }

        /// <summary>
        /// Getting a list of all reviews from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReviewDTO> GetReviews()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Review, ReviewDTO>());
            logger.Log(LogLevel.Info, "Взяты все отзывы из базы данных.");
            return Mapper.Map<IEnumerable<Review>, List<ReviewDTO>>(Database.Reviews.GetAll());
        }

        /// <summary>
        /// Getting a review from the database for further work with it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReviewDTO GetReview(int? id)
        {
            if (id == null)
                throw new ValidationException("Не установлено id номера", "");
            var review = Database.Reviews.Get(id.Value);
            if (review == null)
                throw new ValidationException("Комната не найдена", "");
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Review, ReviewDTO>());
            logger.Log(LogLevel.Info, "Взят отзыв №" + id + " из базы данных.");
            return Mapper.Map<Review, ReviewDTO>(review);
        }

        /// <summary>
        /// Adding a new review to the database
        /// </summary>
        /// <param name="reviewDTO"></param>
        public void SendReview(ReviewDTO reviewDTO)
        {
            Review newReview = new Review
            {
                PublDate = reviewDTO.PublDate,
                Text = reviewDTO.Text,
                UserEmail= reviewDTO.UserEmail
            };
            Database.Reviews.Create(newReview);
            Database.Save();
            logger.Log(LogLevel.Info, "Отзыв №" + reviewDTO.Id + " помещен в базу данных.");
        }

        /// <summary>
        /// Deleting a review from the database
        /// </summary>
        /// <param name="id"></param>
        public void DeleteReview(int id)
        {
            Database.Reviews.Delete(id);
            Database.Save();
            logger.Log(LogLevel.Info, "Отзыв №" + id + " удален из базы данных.");
        }
    }
}
