using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelServices.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HotelServices.DAL.EF
{
    /// <summary>
    /// Data context class
    /// </summary>
    public class HotelServicesContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<BookingApplication> BookingApplications { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DbSet<ClientProfile> ClientProfiles { get; set; }

        /// <summary>
        /// Default constructor for initialization the database
        /// </summary>
        static HotelServicesContext()
        {
            Database.SetInitializer<HotelServicesContext>(new HotelServicesDbInitializer());
        }

        /// <summary>
        /// Сonstructor which gets the name of the connection
        /// </summary>
        /// <param name="connectionString"></param>
        public HotelServicesContext(string connectionString):base(connectionString)
        {

        }
    }

    /// <summary>
    /// Data initialization class
    /// </summary>
    public class HotelServicesDbInitializer:DropCreateDatabaseIfModelChanges<HotelServicesContext>
    {
        /// <summary>
        /// To seed data into database
        /// </summary>
        /// <param name="db"></param>
        protected override void Seed(HotelServicesContext db)
        {
            db.Rooms.Add(new Room {
                Class = "Standart",
                Seats = 2,
                Status = "Free",
                Price = 900,
                ImageURL = "/Content/images/rooms/standart.jpg"
            });
            db.Rooms.Add(new Room
            {
                Class = "Luxe",
                Seats = 2,
                Status = "Occupied",
                Price = 1800,
                ImageURL = "/Content/images/rooms/double.jpg"
            });
            db.Rooms.Add(new Room
            {
                Class = "Apartments",
                Seats = 4,
                Status = "Free",
                Price = 1400,
                ImageURL = "/Content/images/rooms/apartaments.jpg"
            });
            db.Rooms.Add(new Room
            {
                Class = "Luxe",
                Seats = 2,
                Status = "Occupied",
                Price = 2000,
                ImageURL = "/Content/images/rooms/luxe.jpg"
            });            
            db.Rooms.Add(new Room
            {
                Class = "Standart",
                Seats = 2,
                Status = "Free",
                Price = 1100,
                ImageURL = "/Content/images/rooms/twin.jpg"
            });
            db.Rooms.Add(new Room
            {
                Class = "Family",
                Seats = 5,
                Status = "Free",
                Price = 1600,
                ImageURL = "/Content/images/rooms/family.jpg"
            });
            db.Rooms.Add(new Room
            {
                Class = "Business",
                Seats = 3,
                Status = "Free",
                Price = 1650,
                ImageURL = "/Content/images/rooms/business.jpg"
            });
            db.Rooms.Add(new Room
            {
                Class = "President",
                Seats = 2,
                Status = "Free",
                Price = 3000,
                ImageURL = "/Content/images/rooms/president.jpg"
            });
            db.Rooms.Add(new Room
            {
                Class = "Standart",
                Seats = 2,
                Status = "Occupied",
                Price = 900,
                ImageURL = "/Content/images/rooms/double2.jpg"
            });
            db.Rooms.Add(new Room
            {
                Class = "Apartaments",
                Seats = 5,
                Status = "Occupied",
                Price = 1350,
                ImageURL = "/Content/images/rooms/apartaments2.jpg"
            });
            db.Rooms.Add(new Room
            {
                Class = "Standart",
                Seats = 3,
                Status = "Occupied",
                Price = 1300,
                ImageURL = "/Content/images/rooms/triple.jpg"
            });
            db.Rooms.Add(new Room
            {
                Class = "Standart",
                Seats = 2,
                Status = "Free",
                Price = 1000,
                ImageURL = "/Content/images/rooms/twin3.jpg"
            });
            db.Rooms.Add(new Room
            {
                Class = "President",
                Seats = 2,
                Status = "Free",
                Price = 2800,
                ImageURL = "/Content/images/rooms/president2.jpg"
            });
            db.Rooms.Add(new Room
            {
                Class = "Standart",
                Seats = 2,
                Status = "Occupied",
                Price = 800,
                ImageURL = "/Content/images/rooms/standart2.jpg"
            });
            db.Rooms.Add(new Room
            {
                Class = "Standart",
                Seats = 2,
                Status = "Occupied",
                Price = 1000,
                ImageURL = "/Content/images/rooms/twin2.jpg"
            });
            db.Rooms.Add(new Room
            {
                Class = "Business",
                Seats = 2,
                Status = "Occupied",
                Price = 1500,
                ImageURL = "/Content/images/rooms/business2.jpg"
            });
            db.Reviews.Add(new Review
            {
                PublDate = DateTime.Now,
                UserEmail = "Григорий",
                Text = "Хорошее обслуживание, прекрасный номер. Спасибо!"
            });
            db.Reviews.Add(new Review
            {
                PublDate = DateTime.Now,
                UserEmail = "Наталья",
                Text = "Остались по настоящему довольны посещением данного отеля. Из плюсов: " +
                "всегда всё чистое, приятный персонал, вкуснейшие завтраки и ужины. Единственный минус - " +
                "вид из окна был не на сторону моря( Но в следующий раз исправим это."
            });
            db.Reviews.Add(new Review
            {
                PublDate = DateTime.Now,
                UserEmail = "Сергей",
                Text = "Средний по всем показателям, но на пару-тройку дней для бизнес поездки самое то. Оставался в " +
                "номере Бизнес-класса."
            });
            db.Reviews.Add(new Review
            {
                PublDate = DateTime.Now,
                UserEmail = "Виктория",
                Text = "Прекрасный чуткий персонал, отличный номер. Разве можно желать большего? Рекомендую!"
            });
            base.Seed(db);
        }
    }
}
