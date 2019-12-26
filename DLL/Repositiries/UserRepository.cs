using DLL.IRepositories;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Repositiries
{
    public class UserRepository :  Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
        public IEnumerable<User> GetAll()
        {
            return Database.Users;
        }

        public User GetById(int id)
        {
            return Database.Users.Find(id);
        }

        public User Insert(User user)
        {
            Database.Users.Add(user);
            Database.SaveChanges();

            return user;
        }
        public void Update(User user)
        {            
            Database.Users.Update(user);
            Database.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            if (user != null)
            {
                Database.Users.Remove(user);
                Database.SaveChanges();
            }
        }
    }
}
