using Domain.Interfaces.Services;
using Domain.Objects.Requests.Card;
using Domain.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Web.Controllers.Base;

namespace Presentation.Web.Controllers
{
    [ApiController, Authorize, Route("Card")]
    public class CardController(ICardService cardService) : ControllerBase
    {
        [HttpGet("Get/{boardId}")]
        public async Task<IActionResult> Get(int boardId)
        {
            try
            {
                return Ok(await cardService.Get(boardId));
            }
            catch (Exception ex)
            {
                return BadRequest(!ex.Message.IsNullOrEmpty() ? ex.Message : ErrorMessage.ErrorGetting);
            }
        }


        [HttpPatch("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(UpdateCardStatusRequest[] updateCardStatusRequest)
        {
            try
            {
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