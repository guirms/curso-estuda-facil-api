using Domain.Interfaces.Services;
using Domain.Objects.Requests.User;
using Domain.Utils.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Web.Controllers.Base;

namespace Presentation.Web.Controllers
{
    [ApiController, Authorize, Route("User")]
    public class UserController(IUserService userService, IValidator<LogInRequest> logInRequestValidator, IValidator<SaveUserRequest> saveUserRequestValidator) : ControllerBase
    {
        [HttpGet("Get/{currentPage}")]
        public async Task<IActionResult> Get(int currentPage, string? userName)
        {
            try
            {
                if (currentPage < 1)
                    throw new InvalidOperationException(ErrorMessage.InvalidPage);

                return Ok(await userService.Get(currentPage, userName));
            }
            catch (Exception ex)
            {
                return BadRequest(!ex.Message.IsNullOrEmpty() ? ex.Message : ErrorMessage.ErrorGetting);
            }
        }

        [HttpPost("Save"), AllowAnonymous]
        public async Task<IActionResult> Save(SaveUserRequest saveUserRequest)
        {
            try
            {
                saveUserRequestValidator.Validate(saveUserRequest);

                await userService.Save(saveUserRequest);

                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(!ex.Message.IsNullOrEmpty() ? ex.Message : ErrorMessage.ErrorSaving);
            }
        }

        [HttpPost("LogIn"), AllowAnonymous]
        public async Task<IActionResult> LogIn(LogInRequest logInRequest)
        {
            try
            {
                logInRequestValidator.Validate(logInRequest);

                return Ok(await userService.LogIn(logInRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(!ex.Message.IsNullOrEmpty() ? ex.Message : ErrorMessage.ErrorLoggingIn);
            }
        }
    }
}