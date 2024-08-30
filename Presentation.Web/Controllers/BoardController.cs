using Domain.Interfaces.Services;
using Domain.Objects.Requests.User;
using Domain.Utils.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Web.Controllers.Base;

namespace Presentation.Web.Controllers
{
    [ApiController, Authorize, Route("Board")]
    public class BoardController(IBoardService boardService, IValidator<SaveBoardRequest> saveBoardRequestValidator, IValidator<UpdateBoardRequest> updateBoardRequestValidator) : ControllerBase
    {
        [HttpGet("Get/{currentPage}")]
        public async Task<IActionResult> Get(int currentPage, string? boardTheme)
        {
            try
            {
                if (currentPage < 1)
                    throw new InvalidOperationException(ErrorMessage.InvalidPage);

                return Ok(await boardService.Get(currentPage, boardTheme));
            }
            catch (Exception ex)
            {
                return BadRequest(!ex.Message.IsNullOrEmpty() ? ex.Message : ErrorMessage.ErrorGetting);
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(SaveBoardRequest saveBoardRequest)
        {
            try
            {
                saveBoardRequestValidator.Validate(saveBoardRequest);

                await boardService.Save(saveBoardRequest);

                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(!ex.Message.IsNullOrEmpty() ? ex.Message : ErrorMessage.ErrorSaving);
            }
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> Update(UpdateBoardRequest updateBoardRequest)
        {
            try
            {
                updateBoardRequestValidator.Validate(updateBoardRequest);

                await boardService.Update(updateBoardRequest);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(!ex.Message.IsNullOrEmpty() ? ex.Message : ErrorMessage.ErrorUpdating);
            }
        }

        [HttpDelete("Delete/{boardId}")]
        public async Task<IActionResult> Delete(int boardId)
        {
            try
            {
                await boardService.Delete(boardId);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(!ex.Message.IsNullOrEmpty() ? ex.Message : ErrorMessage.ErrorDeleting);
            }
        }
    }
}