using Microsoft.Web.Http;
using System.Collections.Generic;
using System.Web.Http;

namespace WebAPIVersion.V2.Controllers
{
    [ApiVersion("2.0")]
    [RoutePrefix("api/v{api-version:apiVersion}/Values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [Route]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1 v2", "value2 v2" };
        }

        // GET api/values/5
        [Route("{id}")]
        public string Get(int id)
        {
            return "value v2";
        }

        // POST api/values
        [Route]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [Route("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [Route("{id}")]
        public void Delete(int id)
        {
        }
    }
}
