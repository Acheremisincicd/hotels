using HotelServices.DAL.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.DAL.Identity
{
    /// <summary>
    /// Class for managing user data and authentication
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        /// <summary>
        /// Constructor with one parameter
        /// </summary>
        /// <param name="store"></param>
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {

        }
    }
}
