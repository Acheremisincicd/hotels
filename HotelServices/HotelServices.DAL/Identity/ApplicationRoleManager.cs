using HotelServices.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.DAL.Identity
{
    /// <summary>
    /// Class for managing a role of user
    /// </summary>
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        /// <summary>
        /// Constructor with one parameter
        /// </summary>
        /// <param name="store"></param>
        public ApplicationRoleManager(RoleStore<ApplicationRole> store) : base(store)
        {

        }
    }
}
