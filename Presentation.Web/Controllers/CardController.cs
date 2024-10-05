using Domain.Interfaces.Services;
using Domain.Objects.Requests.Card;
using Domain.Utils.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Web.Controllers.Base;

namespace Presentation.Web.Controllers
{
    [ApiController, Authorize, Route("Card")]
    public class CardController(ICardService cardService, IValidator<IEnumerable<UpdateCardStatusRequest>> updateCardStatusRequestValidator) : ControllerBase
    {
        [HttpGet("Get/{boardId}")]
        public async Task<IActionResult> Get(int boardId, string? cardName)
        {
            try
            {
                return Ok(await cardService.Get(boardId, cardName));
            }
            catch (Exception ex)
            {
                return BadRequest(!ex.Message.IsNullOrEmpty() ? ex.Message : ErrorMessage.ErrorGetting);
            }
        }


        [HttpPatch("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(IEnumerable<UpdateCardStatusRequest> updateCardStatusRequest)
        {
            try
            {
                updateCardStatusRequestValidator.Validate(updateCardStatusRequest);

                await cardService.UpdateStatus(updateCardStatusRequest);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(!ex.Message.IsNullOrEmpty() ? ex.Message : ErrorMessage.ErrorUpdating);
            }
        }
    }
}