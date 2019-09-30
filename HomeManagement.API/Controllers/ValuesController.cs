using HomeManagement.API.Filters;
using HomeManagement.Data;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HomeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [Persistable]
    public class ValuesController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITransactionRepository transactionRepository;

        public ValuesController(IUserRepository userRepository, ITransactionRepository transactionRepository)
        {
            this.userRepository = userRepository;
            this.transactionRepository = transactionRepository;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
