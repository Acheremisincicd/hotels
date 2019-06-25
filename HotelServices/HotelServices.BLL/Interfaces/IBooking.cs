using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelServices.BLL.DTO;

namespace HotelServices.BLL.Interfaces
{
    /// <summary>
    /// Interface for working with bookings from database using IUnitOfWork 
    /// </summary>
    public interface IBooking
    {
        RoomDTO GetRoom(int? id);
        IEnumerable<RoomDTO> GetRooms();
        void Book(BookingDTO bookingDTO);
        IEnumerable<BookingDTO> GetBookings();
        void DeleteBooking(int id);
        BookingDTO GetBooking(int? id);
        void UpdateBooking(BookingDTO bookingDTO);
        void MessageAboutCancel(string UserEmail, int id);
        void MessageAboutCancelIfPaid(string UserEmail, int id);
        void Dispose();
    }
}
