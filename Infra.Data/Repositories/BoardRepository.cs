using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Objects.Responses.Asset;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class BoardRepository(ICardRepository cardRepository, SqlContext context, IMapper mapper) : BaseSqlRepository<Board>(context), IBoardRepository
    {
        public async Task DeleteByUserId(int boardId, int userId)
        {
            var board = await GetByIdAndUserId(boardId, userId)
                ?? throw new InvalidOperationException("Board não encontrado");

            _typedContext.Remove(board);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GetBoardResultsResponse>?> GetBoardResults(int userId, int currentPage, string? boardTheme, int takeQuantity = 10)
        {
            var query = _typedContext
                    .AsNoTracking()
                    .Where(b => b.UserId == userId)
                    .OrderByDescending(b => b.BoardId)
                    .Skip((currentPage - 1) * takeQuantity)
                    .Take(takeQuantity);

            if (boardTheme != null)
                query = query.Where(b => b.Theme.Contains(boardTheme));

            return await mapper.ProjectTo<GetBoardResultsResponse>(query).ToListAsync();
        }

        public async Task<Board?> GetByIdAndUserId(int boardId, int userId) =>
            await _typedContext.FirstOrDefaultAsync(b => b.BoardId == boardId && b.UserId == userId);

        public async Task<bool> HasBoardWithSameNameAndUserId(string name, int userId, int? boardId)
        {
            var query = _typedContext
                .AsNoTracking()
                .Where(b => b.Name == name && b.UserId == userId);

            if (boardId.HasValue)
                query = query.Where(b => b.BoardId != boardId);

            return await query.AnyAsync();
        }

        public async Task<int> SaveBoardAndCards(Board board, IEnumerable<Card> cards)
        {
            try
            {
                await StartTransaction();

                await Save(board);

                await cardRepository.SaveMany(cards);

                await CommitTransaction();

                return board.BoardId;
            }
            catch
            {
                await RollbackTransaction();
                throw;
            }
        }
    }
}
