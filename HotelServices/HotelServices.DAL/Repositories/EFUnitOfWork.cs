using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelServices.DAL.Interfaces;
using HotelServices.DAL.Entities;
using HotelServices.DAL.EF;
using HotelServices.DAL.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HotelServices.DAL.Repositories
{
    /// <summary>
    /// UnitOfWork class, which simplify the work with various repositories and gives confidence that all repositories
    /// will use the same data context; this class encapsulates all managers to work with entities in properties and 
    /// stores the general context of the data.
    /// </summary>
    public class EFUnitOfWork: IUnitOfWork
    {
        private HotelServicesContext db;
        private RoomRepository roomRepository;
        private BookingApplicationRepository bookingApplicationRepository;
        private BookingRepository bookingRepository;
        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private MessageRepository messageRepository;
        private ReviewRepository reviewRepository;
        private IClientManager clientManager;

        /// <summary>
        /// Constructor with the name of the connection, which will then be passed to the data context constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public EFUnitOfWork(string connectionString)
        {
            db = new HotelServicesContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            clientManager = new ClientManager(db);
        }

        public IRepository<Room> Rooms
        {
            get
            {
                if (roomRepository == null)
                    roomRepository = new RoomRepository(db);
                return roomRepository;
            }
            set
            { }
        }

        public IRepository<BookingApplication> BookingApplications
        {
            get
            {
                if (bookingApplicationRepository == null)
                    bookingApplicationRepository = new BookingApplicationRepository(db);
                return bookingApplicationRepository;
            }
            set
            { }
        }

        public IRepository<Booking> Bookings
        {
            get
            {
                if (bookingRepository == null)
                    bookingRepository = new BookingRepository(db);
                return bookingRepository;
            }
            set
            { }
        }

        public IRepository<Message> Messages
        {
            get
            {
                if (messageRepository == null)
                    messageRepository = new MessageRepository(db);
                return messageRepository;
            }
            set
            { }
        }

        public IRepository<Review> Reviews
        {
            get
            {
                if (reviewRepository == null)
                    reviewRepository = new ReviewRepository(db);
                return reviewRepository;
            }
            set
            { }
        }

        public ApplicationUserManager UserManager
        {
            get { return userManager; }
        }

        public IClientManager ClientManager
        {
            get { return clientManager; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return roleManager; }
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
