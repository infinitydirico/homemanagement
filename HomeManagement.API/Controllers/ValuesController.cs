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
        private readonly Data.Repositories.IChargeRepository chargeRepository;

        public ValuesController(IUserRepository userRepository, Data.Repositories.IChargeRepository chargeRepository)
        {
            this.userRepository = userRepository;
            this.chargeRepository = chargeRepository;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var firstCharge = chargeRepository.FirstOrDefault();
            firstCharge.Date = DateTime.Now;
            chargeRepository.Update(firstCharge);
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
