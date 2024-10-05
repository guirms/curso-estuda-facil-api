using Domain.Models;
using Domain.Objects.Responses.Card;

namespace Domain.Interfaces.Repositories
{
    public interface ICardRepository : IBaseSqlRepository<Card>
    {
        Task<IEnumerable<Card>?> GetByIdAndUserId(IEnumerable<int> cardId, int userId);
        Task<IEnumerable<GetCardResultsResponse>?> GetCardResults(int boardId, int userId, string? cardName);
    }
}
