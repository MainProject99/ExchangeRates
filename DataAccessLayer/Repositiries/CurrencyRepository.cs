using DataAccessLayer.IRepositories;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Repositiries
{
    public class CurrencyRepository : Repository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public virtual IQueryable<Currency> Get(string includeProperties = "")
        {
            IQueryable<Currency> query = Database.Currencies
                                               .Include(x => x.User);
                                                

            foreach (var includeProperty in
                includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query;
        }
        public Currency GetById(int id)
        {
            var result = (from r in Database.Currencies
                          .Include(c => c.User)
                          where r.UserId == id
                          select r)
                          .FirstOrDefault();
            return result;

        }

    }
}
