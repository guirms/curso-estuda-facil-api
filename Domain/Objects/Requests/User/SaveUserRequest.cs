using Domain.Utils.Helpers;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Domain.Objects.Requests.User
{
    public record SaveUserRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Document { get; set; }
        public required string Password { get; set; }
        public required string PasswordConfirmation { get; set; }
    }

    public partial class SaveUserRequestValidator : AbstractValidator<SaveUserRequest>
    {
        [GeneratedRegex(@"[!@#$%^&*(),.?""':{}|<>]")]
        private static partial Regex SpecialCharacterPattern();

        public SaveUserRequestValidator()
        {
            RuleFor(s => s)
                .Must(HaveValidFields);
        }

        private bool HaveValidFields(SaveUserRequest saveUserRequest)
        {
            var password = saveUserRequest.Password;
            var name = saveUserRequest.Name;
            var document = saveUserRequest.Document;

            if (password != saveUserRequest.PasswordConfirmation)
                throw new ValidationException("As senhas não são iguais");

            if (name.IsNullOrEmpty() || name.Length > 50)
                throw new ValidationException("Nome inválido");

            if (!saveUserRequest.Email.IsValidEmail())
                throw new ValidationException("Email inválido");

            if (!document.IsValidCpf())
                throw new ValidationException("Documento inválido");

            IsValidPassword(password);

            return true;
        }

        private static bool IsValidPassword(string password)
        {
            if (password.IsNullOrEmpty() || password.Length < 8 || password.Length > 50)
                throw new ValidationException("A senha precisa ter entre 8 e 50 caracteres");

            if (!SpecialCharacterPattern().IsMatch(password))
                throw new ValidationException("A senha precisa ter caracteres especiais");

            if (!password.Any(char.IsLower) || !password.Any(char.IsUpper))
                throw new ValidationException("A senha precisa possuir caracteres minúsculos e maiúsculos");

            return true;
        }
    }
}
