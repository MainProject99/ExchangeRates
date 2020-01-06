using DataAccessLayer.IRepositories;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repositiries
{
    public class CurrencyRepository : Repository<Currencies>, ICurrencyRepository
    {
        public CurrencyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
