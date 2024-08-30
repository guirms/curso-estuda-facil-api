using Domain.Utils.Helpers;
using FluentValidation;

namespace Domain.Objects.Requests.User
{
    public record SaveBoardRequest
    {
        public required string Name { get; set; }
        public required string Theme { get; set; }
        public required DateTime ExamDateTime { get; set; }
    }

    public class SaveBoardRequestValidator : AbstractValidator<SaveBoardRequest>
    {
        public SaveBoardRequestValidator()
        {
            RuleFor(u => u)
                .Must(HaveValidFields);
        }

        private static bool HaveValidFields(SaveBoardRequest saveBoardRequest)
        {
            if (saveBoardRequest.Name.IsNullOrEmpty() || saveBoardRequest.Name.Length > 50)
                throw new ValidationException("Nome inválido");

            if (saveBoardRequest.Theme.IsNullOrEmpty() || saveBoardRequest.Theme.Length > 50)
                throw new ValidationException("Tema inválido");

            if (saveBoardRequest.ExamDateTime < DateTime.Now)
                throw new ValidationException("A data de exame não pode ser menor que a data atual");

            return true;
        }
    }
}
