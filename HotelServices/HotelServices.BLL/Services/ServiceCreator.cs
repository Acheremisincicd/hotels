using HotelServices.BLL.Interfaces;
using HotelServices.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.Services
{
    /// <summary>
    /// Abstract factory
    /// </summary>
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService(string connection)
        {
            return new UserService(new EFUnitOfWork(connection));
        }
    }
}
