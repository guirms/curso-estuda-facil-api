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

    public partial class UserRequestValidator : AbstractValidator<SaveUserRequest>
    {
        [GeneratedRegex(@"[!@#$%^&*(),.?""':{}|<>]")]
        private static partial Regex specialCharacterPattern();

        public UserRequestValidator()
        {
            RuleFor(u => u)
                .Must(HaveValidFields);
        }

        private bool HaveValidFields(SaveUserRequest saveUserRequest)
        {
            var password = saveUserRequest.Password;
            var name = saveUserRequest.Name;
            var document = saveUserRequest.Document;

            if (password != saveUserRequest.PasswordConfirmation)
                throw new ValidationException("PasswordsAreNotTheSame");

            if (name.IsNullOrEmpty() || name.Length > 50)
                throw new ValidationException("InvalidUsername");

            if (!saveUserRequest.Email.IsValidEmail())
                throw new ValidationException("InvalidEmail");

            if (!document.IsValidCpf())
                throw new ValidationException("InvalidDocument");

            if (!IsValidPassword(password))
                throw new ValidationException("InvalidUserPassword");

            return true;
        }

        public static bool IsValidPassword(string password)
        {
            if (password.IsNullOrEmpty() || password.Length < 8 || password.Length > 50)
                throw new ValidationException("PasswordMustHaveValidLength");

            if (!specialCharacterPattern().IsMatch(password))
                throw new ValidationException("PasswordMustHaveSpecialCharacter");

            if (!password.Any(char.IsLower) || !password.Any(char.IsUpper))
                throw new ValidationException("PasswordMustContainUppercaseAndLowercase");

            return true;
        }
    }
}
