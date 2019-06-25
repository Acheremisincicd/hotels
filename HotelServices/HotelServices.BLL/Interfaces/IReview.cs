using HotelServices.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.Interfaces
{
    /// <summary>
    /// Interface for working with reviews from database using IUnitOfWork 
    /// </summary>
    public interface IReview
    {
        void SendReview(ReviewDTO bookingDto);
        void DeleteReview(int id);
        IEnumerable<ReviewDTO> GetReviews();
        ReviewDTO GetReview(int? id);        
    }
}
