using DLL.IRepositories;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Repositiries
{
    public class CurrencyFromRepository : Repository<CurrencyFrom>, IRepository<CurrencyFrom>
    {
        public CurrencyFromRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
