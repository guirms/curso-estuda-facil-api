using Domain.Models.Enums.Task;
using FluentValidation;

namespace Domain.Objects.Requests.Card
{
    public record UpdateCardStatusRequest
    {
        public int CardId { get; init; }
        public ECardStatus CardStatus { get; init; }
    }

    public partial class UpdateCardStatusRequestValidator : AbstractValidator<IEnumerable<UpdateCardStatusRequest>>
    {
        public UpdateCardStatusRequestValidator()
        {
            RuleFor(u => u)
                .Must(HaveValidFields);
        }

        private bool HaveValidFields(IEnumerable<UpdateCardStatusRequest> updateCardStatusRequest)
        {
            var duplicatedIds = updateCardStatusRequest
                .GroupBy(u => u.CardId)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();

            if (duplicatedIds.Any())
                throw new ValidationException($"Ids duplicados: {string.Join(", ", duplicatedIds)}");

            foreach (var cardStatusItem in updateCardStatusRequest)
            {
                if (!Enum.IsDefined(typeof(ECardStatus), cardStatusItem.CardStatus))
                    throw new ValidationException("Status inválido");
            }

            return true;
        }
    }
}
