using HotelServices.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.DAL.Interfaces
{
    /// <summary>
    /// Interface for interacting with client in database
    /// </summary>
    public interface IClientManager : IDisposable
    {
        void Create(ClientProfile item);
        ClientProfile GetClient(string id);
        void Update(ClientProfile client);
    }
}
