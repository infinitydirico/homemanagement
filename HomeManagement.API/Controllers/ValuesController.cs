using System;
using HomeManagement.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IUserRepository userRepository;

        public ValuesController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                userRepository.Add(new Domain.User
                {
                    Email = DateTime.Now.ToString("hh.mm.ss"),
                    Password = DateTime.Now.ToString("hh.mm.ss")
                });

                return Ok(userRepository.GetAll());
            }
            catch (Exception ex)
            {
                return Forbid(ex.Message);                
            }
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
