using Domain.Models;
using Domain.Objects.Responses.Asset;

namespace Domain.Interfaces.Repositories
{
    public interface IBoardRepository : IBaseSqlRepository<Board>
    {
        Task DeleteByUserId(int boardId, int userId);
        Task<IEnumerable<GetBoardResultsResponse>?> GetBoardResults(int userId, int currentPage, string? boardTheme, int takeQuantity = 10);
        Task<Board?> GetByIdAndUserId(int boardId, int userId);
        Task<bool> HasBoardWithSameNameAndUserId(string name, int userId, int? boardId = null);
        Task<int> SaveBoardAndCards(Board board, IEnumerable<Card> cards);
    }
}
