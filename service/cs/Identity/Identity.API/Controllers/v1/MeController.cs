using AutoWrapper.Wrappers;
using FluentValidation;
using FluentValidation.AspNetCore;
using Identity.API.Models.Request;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.Domain.Interfaces;
using Identity.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
            var user = await _userRepository.GetByIdAsync(id);

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
            if (createUserRequest == null)
            {
                throw new ApiProblemDetailsException("Unable to create user", StatusCodes.Status400BadRequest);
            }

            var result = await _validator.ValidateAsync(createUserRequest);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState, null);
                throw new ApiProblemDetailsException(ModelState);
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

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(string id, [FromBody] JsonPatchDocument<User> userPatchDoc)
        {
            //shouldn't make it here unless we have confirmed the oid is the users id from a filter most likely
            var currentUser = await _userRepository.GetByIdAsync(id);

            if (currentUser == null)
            {
                return NotFound();
            }
            
            userPatchDoc.ProtectedApplyTo(
                currentUser, 
                (int) currentUser.Role,
                error => throw new ApiProblemDetailsException(error.ErrorMessage, StatusCodes.Status400BadRequest));

            return Ok(currentUser);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var userToDelete = await _userRepository.GetByIdAsync(id);

            if (userToDelete == null)
            {
                return NotFound();
            }

            //soft delete
            userToDelete.Active = false;
            await _userRepository.SaveAsync(userToDelete);

            return Ok();
        }
    }
}

