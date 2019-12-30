using DLL.IRepositories;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Repositiries
{
    public class CurrencyRepository : Repository<Currencies>, ICurrencyRepository
    {
        public CurrencyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
