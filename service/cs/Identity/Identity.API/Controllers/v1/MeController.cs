using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Identity.API.Models.Request;
using Identity.Data.Repositories;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.Resource;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Identity.API.Controllers.v1
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class MeController : Controller
    {
        private readonly IValidator<CreateUserRequest> _validator;
        private readonly IUserRepository _userRepository;

        public MeController(IValidator<CreateUserRequest> validator, IUserRepository userRepository)
        {
            _validator = validator;
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
        public async Task<IActionResult> Post([FromBody] CreateUserRequest createUserRequest)
        {
            var result = await _validator.ValidateAsync(createUserRequest);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(r => r.ErrorMessage);
                var errors = errorMessages.Aggregate((a, b) => a + ", " + b);
                return BadRequest(errors);
            }

            var user = new User()
            {
                Oid = createUserRequest.Oid,
                Role = Role.User
            };
            var savedUser = await _userRepository.SaveAsync(user);

            return Ok(savedUser);
        }

        // create is user filter
        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            //shouldn't make it here unless we have confirmed the oid is the users id from a filter most likely
            var currentUser = await _userRepository.GetByOidAsync(id);

            if (currentUser == null)
            {
                return NotFound();
            }
            
            currentUser.Role = updateUserRequest.Role ?? currentUser.Role;
            currentUser.Active = updateUserRequest.Active ?? currentUser.Active;
            var updatedUser = await _userRepository.SaveAsync(currentUser);

            return Ok(updatedUser);
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

