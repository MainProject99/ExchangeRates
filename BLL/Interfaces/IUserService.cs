using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
    }
}
