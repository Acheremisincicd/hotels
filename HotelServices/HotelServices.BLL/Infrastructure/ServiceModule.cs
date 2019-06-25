using HotelServices.DAL.Interfaces;
using HotelServices.DAL.Repositories;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.Infrastructure
{
    /// <summary>
    /// Class for binding IUnitOfWork with EFUnitOfWork using Ninject
    /// </summary>
    public class ServiceModule: NinjectModule
    {
        private string connectionString;
        /// <summary>
        /// Сonstructor which gets the name of the connection
        /// </summary>
        /// <param name="connection"></param>
        public ServiceModule(string connection)
        {
            connectionString = connection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(connectionString);
        }
    }
}
