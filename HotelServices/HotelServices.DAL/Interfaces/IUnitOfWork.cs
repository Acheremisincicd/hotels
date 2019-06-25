using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelServices.DAL.Entities;
using HotelServices.DAL.Identity;

namespace HotelServices.DAL.Interfaces
{
    /// <summary>
    /// UnitOfWork interface
    /// </summary>
    public interface IUnitOfWork: IDisposable
    {
        IRepository<Room> Rooms { get; set; }
        IRepository<BookingApplication> BookingApplications { get; set; }
        IRepository<Booking> Bookings { get; set; }
        IRepository<Message> Messages { get; set; }
        IRepository<Review> Reviews { get; set; }
        void Save();
        ApplicationUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }
        Task SaveAsync();
    }
}
