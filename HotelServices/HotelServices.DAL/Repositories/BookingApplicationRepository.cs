using HotelServices.DAL.EF;
using HotelServices.DAL.Entities;
using HotelServices.DAL.Interfaces;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.DAL.Repositories
{
    /// <summary>
    /// Repository class for interacting with booking applications in database
    /// </summary>
    class BookingApplicationRepository : IRepository<BookingApplication>
    {
        private HotelServicesContext db;

        public BookingApplicationRepository(HotelServicesContext context)
        {
            this.db = context;
        }

        public IEnumerable<BookingApplication> GetAll()
        {
            return db.BookingApplications;
        }

        public BookingApplication Get(int? id)
        {
            return db.BookingApplications.Find(id);
        }

        public void Create(BookingApplication bookingApplication)
        {
            db.BookingApplications.Add(bookingApplication);
        }

        public void Update(BookingApplication bookingApplication)
        {
            db.Entry(bookingApplication).State = EntityState.Modified;
        }

        public IEnumerable<BookingApplication> Find(Func<BookingApplication, Boolean> predicate)
        {
            return db.BookingApplications.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            BookingApplication bookingApplication = db.BookingApplications.Find(id);
            if (bookingApplication != null)
                db.BookingApplications.Remove(bookingApplication);
        }
    }
}
