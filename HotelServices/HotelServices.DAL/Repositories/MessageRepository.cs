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
    /// Repository class for interacting with messages in database
    /// </summary>
    class MessageRepository : IRepository<Message>
    {
        private HotelServicesContext db;

        public MessageRepository(HotelServicesContext context)
        {
            this.db = context;
        }

        public IEnumerable<Message> GetAll()
        {
            return db.Messages;
        }

        public Message Get(int? id)
        {
            return db.Messages.Find(id);
        }

        public void Create(Message message)
        {
            db.Messages.Add(message);
        }

        public void Update(Message message)
        {
            db.Entry(message).State = EntityState.Modified;
        }

        public IEnumerable<Message> Find(Func<Message, Boolean> predicate)
        {
            return db.Messages.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Message message = db.Messages.Find(id);
            if (message != null)
                db.Messages.Remove(message);
        }
    }
}
