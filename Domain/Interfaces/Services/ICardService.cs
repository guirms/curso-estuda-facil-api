using Domain.Objects.Requests.Card;
using Domain.Objects.Responses.Card;

namespace Domain.Interfaces.Services
{
    public interface ICardService
    {
        Task<IEnumerable<GetCardsResponse>?> Get(int boardId);
        Task UpdateStatus(UpdateCardStatusRequest[] updateCardStatusRequest);
    }
}
