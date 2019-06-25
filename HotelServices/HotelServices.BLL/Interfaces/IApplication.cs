using HotelServices.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.Interfaces
{
    /// <summary>
    /// Interface for working with applications from database using IUnitOfWork 
    /// </summary>
    public interface IApplication
    {
        void SendApplication(BookingApplicationDTO bookingDto);
        IEnumerable<BookingApplicationDTO> GetApplications();
        void BookForManager(BookingDTO bookingDTO);
        void DeleteApp(int id);
        BookingApplicationDTO GetApplication(int? id);
        void SendRefusal(int ApplicationId);
        IEnumerable<MessageDTO> GetMessages();
    }
}
