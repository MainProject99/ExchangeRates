using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyWebAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class CurrencyController : ApiController
    {
        // GET: api/
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/student/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/student
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/student/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/student/5
        public void Delete(int id)
        {
        }
    }
}