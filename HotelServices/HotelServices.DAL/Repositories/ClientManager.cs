using HotelServices.DAL.EF;
using HotelServices.DAL.Entities;
using HotelServices.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.DAL.Repositories
{
    /// <summary>
    /// Repository class for interacting with users in database
    /// </summary>
    public class ClientManager : IClientManager
    {
        public HotelServicesContext Database { get; set; }
        public ClientManager(HotelServicesContext db)
        {
            Database = db;
        }

        public void Create(ClientProfile item)
        {
            Database.ClientProfiles.Add(item);
            Database.SaveChanges();
        }

        public ClientProfile GetClient(string id)
        {
            return Database.ClientProfiles.Find(id);
        }

        public void Update(ClientProfile client)
        {
            Database.Entry(client).State = EntityState.Modified;
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
