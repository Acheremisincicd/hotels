using HotelServices.BLL.DTO;
using HotelServices.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.Interfaces
{
    /// <summary>
    /// Interface for working with users from database using IUnitOfWork 
    /// </summary>
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task SetInitialData(UserDTO adminDto, UserDTO managerDto, List<string> roles);
        UserDTO GetUser(string UserEmail);
        void UpdateClient(UserDTO userDTO);
    }
}
