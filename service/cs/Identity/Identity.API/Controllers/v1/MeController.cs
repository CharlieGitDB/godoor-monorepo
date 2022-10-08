using FluentValidation;
using FluentValidation.AspNetCore;
using Identity.API.Models.Request;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Identity.API.Controllers.v1
{
    [Authorize]
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
        
        // TODO: should this even exist? If so it needs to be locked to admins
        // GET: api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _userRepository.GetAllAsync());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var user = await _userRepository.GetByOidAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        
        // TODO: this needs to only be able to be hit by a azure function that runs on azure b2c creation
        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateUserRequest createUserRequest)
        {
            var result = await _validator.ValidateAsync(createUserRequest);

            if (!result.IsValid)
            {
                return ValidationProblem(this.ModelState);
            }

            var user = new User
            {
                Oid = createUserRequest.Oid,
                Role = Role.User,
                CreatedByOid = User.GetObjectId(),
                ModifiedByOid = User.GetObjectId()
            };
            
            var savedUser = await _userRepository.SaveAsync(user);

            return Ok(savedUser);
        }

        // TODO: create user filter here
        // PUT api/values/5
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(string id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            //shouldn't make it here unless we have confirmed the oid is the users id from a filter most likely
            var currentUser = await _userRepository.GetByOidAsync(id);

            if (currentUser == null)
            {
                return NotFound();
            }
            
            currentUser.Role = updateUserRequest.Role ?? currentUser.Role;
            currentUser.Active = updateUserRequest.Active ?? currentUser.Active;
            currentUser.ModifiedByOid = User.GetObjectId();
            var updatedUser = await _userRepository.SaveAsync(currentUser);

            return Ok(updatedUser);
        }

        // TODO: use user filter here
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var userToDelete = await _userRepository.GetByOidAsync(id);

            if (userToDelete == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteAsync(userToDelete);

            return Ok();
        }
    }
}

