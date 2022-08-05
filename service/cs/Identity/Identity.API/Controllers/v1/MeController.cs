using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.Resource;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Identity.API.Controllers.v1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class MeController : Controller
    {
        private readonly UserRepository _userRepository;

        public MeController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userRepository.GetAllAsync());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // create is user filter
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userToDelete = await _userRepository.GetByIdAsync(id);

            if (userToDelete == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteAsync(userToDelete);

            return Ok();
        }
    }
}

