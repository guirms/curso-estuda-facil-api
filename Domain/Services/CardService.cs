using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Objects.Requests.Card;
using Domain.Objects.Responses.Card;
using Domain.Utils.Constants;

namespace Domain.Services
{
    public class CardService(ICardRepository cardRepository) : ICardService
    {
        public async Task<IEnumerable<GetCardsResponse>?> Get(int boardId, string? cardName)
        {
            var result = await cardRepository.GetCardResults(boardId, Session.UserId, cardName);

            if (result == null || !result.Any())
                throw new InvalidOperationException("Nenhum card encontrado");

            var todoList = result.Where(r => r.TaskStatus == Models.Enums.Task.ECardStatus.ToDo);
            var doingList = result.Where(r => r.TaskStatus == Models.Enums.Task.ECardStatus.Doing);
            var doneList = result.Where(r => r.TaskStatus == Models.Enums.Task.ECardStatus.Done);

            return
            [
                new() {
                    TaskStatus = Models.Enums.Task.ECardStatus.ToDo,
                    Cards = todoList
                },
                new() {
                    TaskStatus = Models.Enums.Task.ECardStatus.Doing,
                    Cards = doingList
                },
                new() {
                    TaskStatus = Models.Enums.Task.ECardStatus.Done,
                    Cards = doneList
                }
            ];
        }

        public async Task UpdateStatus(IEnumerable<UpdateCardStatusRequest> updateCardStatusRequest)
        {
            var cardIds = updateCardStatusRequest.Select(c => c.CardId);

            var cards = await cardRepository.GetByIdAndUserId(cardIds, Session.UserId);

            if (cards == null || !cards.Any() || cards.Count() != updateCardStatusRequest.Count())
                throw new InvalidOperationException("Card(s) não encontrado(s)");

            var currentDateTime = DateTime.Now;

            foreach (var card in cards)
            {
                foreach (var cardId in cardIds)
                {
                    if (card.CardId == cardId)
                    {
                        var newStatus = updateCardStatusRequest.First(c => c.CardId == cardId).CardStatus;

                        card.TaskStatus = newStatus;
                        card.UpdatedAt = currentDateTime;
                    }
                }
            }

            await cardRepository.UpdateMany(cards);
        }
    }
}
