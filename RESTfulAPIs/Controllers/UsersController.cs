using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace RESTfulAPIs.Controllers
{
    [Authorize]
    [ApiController]
    [System.Web.Http.Route("[controller]")]
    public class UsersController : ControllerBase
    {
    }
}
