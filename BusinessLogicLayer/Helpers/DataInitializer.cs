using BusinessLogicLayer.Interfaces;
using DataAccessLayer.IRepositories;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Helpers
{
    public static class DataInitializer
    {
        public static void SeedData(IUserService userService, ICurrencyRepository currencyRepository) 
        {
            SeedCurrencyForUser(userService, currencyRepository); 
        }
        public static void SeedCurrencyForUser(IUserService userService, ICurrencyRepository currencyRepository)
        {
            if (!userService.Exsist("Admin@gmail.com"))
            {
                User user = new  User();
                user.Email = "Admin@gmail.com";
                user.Name = "Admin";
                user.Password = "Admin";

                userService.CreateUser(user,user.Password);

                Currency currency = new Currency();
                currency.UserId = user.Id;
                currency.CurrencyFrom = "GRN";
                currency.CurrencyTo = "USD";
                currencyRepository.Insert(currency);
            }

        }

    }
}
