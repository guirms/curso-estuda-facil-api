using AutoMapper;
using Domain.Interfaces.Externals;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.Enums.Task;
using Domain.Objects.Requests.User;
using Domain.Objects.Responses.Asset;
using Domain.Utils.Constants;
using Domain.Utils.Helpers;

namespace Domain.Services
{
    public class BoardService(IBoardRepository boardRepository, IMapper mapper, IPyAIExternal pyAIExternal) : IBoardService
    {
        public async Task Delete(int boardId) => await boardRepository.DeleteByUserId(boardId, Session.UserId);

        public async Task<IEnumerable<GetBoardResultsResponse>?> Get(int currentPage, string? boardTheme)
        {
            var result = await boardRepository.GetBoardResults(Session.UserId, currentPage, boardTheme);

            if (result == null || !result.Any())
                throw new InvalidOperationException("Nenhum board encontrado");

            return result;
        }

        public async Task Save(SaveBoardRequest saveBoardRequest)
        {
            var userId = Session.UserId;

            if (await boardRepository.HasBoardWithSameNameAndUserId(saveBoardRequest.Name, userId))
                throw new InvalidOperationException("Board com o mesmo nome já cadastrado");

            var board = mapper.Map<Board>(saveBoardRequest);
            board.UserId = userId;

            var currentDateTime = DateTime.Now;

            var daysUntilExam = saveBoardRequest.ExamDateTime.Subtract(currentDateTime).Days;

            var generatedCards = await pyAIExternal.GenerateBoard(saveBoardRequest.Theme, daysUntilExam)
                ?? throw new InvalidOperationException("Erro ao gerar cards, considere reescrever o tema da avaliação!");

            var cards = new List<Card>();
            var order = 0;

            foreach (var cardData in generatedCards)
            {
                foreach (var cardContent in cardData.Contents!)
                {
                    cards.Add(new Card
                    {
                        Name = cardContent.Name,
                        Description = cardContent.Description,
                        StudyDay = cardData.Day,
                        TaskStatus = ECardStatus.ToDo,
                        Order = order,
                        StudyTime = TimeSpan.FromMinutes(cardContent.StudyTime),
                        Board = board,
                        InsertedAt = currentDateTime,
                    });
                    order++;
                }
            }

            await boardRepository.SaveBoardAndCards(board, cards);
        }

        public async Task Update(UpdateBoardRequest updateBoardRequest)
        {
            var userId = Session.UserId;

            var board = await boardRepository.GetByIdAndUserId(updateBoardRequest.BoardId, Session.UserId)
                ?? throw new InvalidOperationException("Board não encontrado");

            if (updateBoardRequest.Theme != null && await boardRepository.HasBoardWithSameNameAndUserId(updateBoardRequest.Theme, userId, board.BoardId))
                throw new InvalidOperationException("Board com o mesmo nome já cadastrado");

            board = updateBoardRequest.MapIgnoringNullProperties(board);

            board.UpdatedAt = DateTime.Now;

            await boardRepository.Update(board);
        }
    }
}
