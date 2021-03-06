﻿using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string email, string password);
        User CreateUser(User user, string password);

        void UpdateUser(User userParam, string password = null);
    }
}
