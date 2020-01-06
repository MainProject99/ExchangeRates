﻿using DataAccessLayer.IRepositories;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repositiries
{
    public class CurrencyFromRepository : Repository<CurrencyFrom>, ICurrencyFromRepository
    {
        public CurrencyFromRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}