using AutoMapper;
using HotelServices.BLL.DTO;
using HotelServices.BLL.Interfaces;
using HotelServices.WEB.Filters;
using HotelServices.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelServices.WEB.Controllers
{
    /// <summary>
    /// Review controller
    /// </summary>
    public class ReviewController : Controller
    {
        IReview review;

        /// <summary>
        /// Constructor with one parameter
        /// </summary>
        /// <param name="r"></param>
        public ReviewController(IReview r)
        {
            review = r;
        }

        /// <summary>
        /// Displays all of the review from the database
        /// </summary>
        /// <returns></returns>
        [ExceptionFilter]
        [HttpGet]
        public ActionResult ShowReviews()
        {
            IEnumerable<ReviewDTO> reviewDtos = review.GetReviews();
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<ReviewDTO, ReviewViewModel>());
            var reviews = Mapper.Map<IEnumerable<ReviewDTO>, List<ReviewViewModel>>(reviewDtos);
            return View(reviews);
        }

        /// <summary>
        /// Gets review view model and adds it to the database
        /// </summary>
        /// <param name="reviewVM"></param>
        /// <returns></returns>
        [ExceptionFilter]
        [Authorize(Roles = "user, manager, admin")]
        [HttpPost]
        public ActionResult AddReview(ReviewViewModel reviewVM)
        {
            if (ModelState.IsValid)
            {
                reviewVM.PublDate = DateTime.Now;
                reviewVM.UserEmail = User.Identity.Name;
                Mapper.Reset();
                Mapper.Initialize(cfg => cfg.CreateMap<ReviewViewModel, ReviewDTO>());
                var reviewDTO = Mapper.Map<ReviewViewModel, ReviewDTO>(reviewVM);
                review.SendReview(reviewDTO);
            }
            else
            {
                return RedirectToAction("ShowReviews");
            }
            return RedirectToAction("ShowReviews");
        }

        [ExceptionFilter]
        [Authorize(Roles = "user, manager, admin")]
        public ActionResult DeleteReview(int id)
        {
            review.DeleteReview(id);
            return RedirectToAction("ShowReviews");
        }
    }
}