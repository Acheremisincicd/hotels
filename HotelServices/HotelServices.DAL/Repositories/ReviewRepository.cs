using HotelServices.DAL.EF;
using HotelServices.DAL.Entities;
using HotelServices.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.DAL.Repositories
{
    /// <summary>
    /// Repository class for interacting with reviews in database
    /// </summary>
    class ReviewRepository : IRepository<Review>
    {
        private HotelServicesContext db;

        public ReviewRepository(HotelServicesContext context)
        {
            this.db = context;
        }

        public IEnumerable<Review> GetAll()
        {
            return db.Reviews;
        }

        public Review Get(int? id)
        {
            return db.Reviews.Find(id);
        }

        public void Create(Review review)
        {
            db.Reviews.Add(review);
        }

        public void Update(Review review)
        {
            db.Entry(review).State = EntityState.Modified;
            db.SaveChanges();
        }

        public IEnumerable<Review> Find(Func<Review, Boolean> predicate)
        {
            return db.Reviews.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review != null)
                db.Reviews.Remove(review);
        }
    }
}
