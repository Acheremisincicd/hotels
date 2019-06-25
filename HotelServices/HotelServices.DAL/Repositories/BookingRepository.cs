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
    /// Repository class for interacting with bookings in database
    /// </summary>
    class BookingRepository : IRepository<Booking>
    {
        private HotelServicesContext db;

        public BookingRepository(HotelServicesContext context)
        {
            this.db = context;
        }

        public IEnumerable<Booking> GetAll()
        {
            return db.Bookings;
        }

        public Booking Get(int? id)
        {
            return db.Bookings.Find(id);
        }

        public void Create(Booking booking)
        {
            db.Bookings.Add(booking);
        }

        public void Update(Booking booking)
        {
            db.Entry(booking).State = EntityState.Modified;
            db.SaveChanges();
        }

        public IEnumerable<Booking> Find(Func<Booking, Boolean> predicate)
        {
            return db.Bookings.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking != null)
                db.Bookings.Remove(booking);
        }
    }
}
