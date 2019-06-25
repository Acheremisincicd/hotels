using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.Interfaces
{
    /// <summary>
    /// Interface of abstract factory
    /// </summary>
    public interface IServiceCreator
    {
        IUserService CreateUserService(string connection);
    }
}
