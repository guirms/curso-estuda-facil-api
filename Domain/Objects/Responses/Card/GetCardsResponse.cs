using Domain.Models.Enums.Task;

namespace Domain.Objects.Responses.Card
{
    public record GetCardsResponse
    {
        public required ECardStatus TaskStatus { get; set; }
        public IEnumerable<GetCardResultsResponse>? Cards { get; set; }
    }
}