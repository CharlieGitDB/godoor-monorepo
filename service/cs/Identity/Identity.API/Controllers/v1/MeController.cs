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
        
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            if (User.GetObjectId() == null)
            {
                return NotFound();
            }
            
            var user = await _userRepository.GetByIdAsync(User.GetObjectId());

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        
        [Authorize("ApiConnectorBasicAuth")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateUserRequest createUserRequest)
        {
            if (createUserRequest == null || createUserRequest.ObjectId == null)
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
                Oid = createUserRequest.ObjectId,
                Role = Role.User,
                CreatedByOid = createUserRequest.ObjectId,
                ModifiedByOid = createUserRequest.ObjectId
            };
            
            var savedUser = await _userRepository.SaveAsync(user);

            return Ok(savedUser);
        }

        [Authorize]
        [HttpPatch]
        public async Task<ActionResult> Patch([FromBody] JsonPatchDocument<User> userPatchDoc)
        {
            if (User.GetObjectId() == null)
            {
                return NotFound();
            }

            var currentUser = await _userRepository.GetByIdAsync(User.GetObjectId());

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

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> Delete()
        {
            var userToDelete = await _userRepository.GetByIdAsync(User.GetObjectId());

            if (userToDelete == null)
            {
                return NotFound();
            }

            //soft delete
            userToDelete.Active = false;
            await _userRepository.SaveAsync(userToDelete);

            //need to kill the users session
            //https://stackoverflow.com/questions/41187094/azure-ad-b2c-sign-out-a-user-from-all-sessions
            
            return Ok();
        }
    }
}

