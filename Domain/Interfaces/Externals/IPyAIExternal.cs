using Domain.Objects.Dto_s.Card;

namespace Domain.Interfaces.Externals
{
    public interface IPyAIExternal
    {
        Task<IEnumerable<CardData>> GenerateBoard(string theme, int daysUntilExam);
    }
}
