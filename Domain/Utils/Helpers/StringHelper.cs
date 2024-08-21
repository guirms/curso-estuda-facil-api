using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace Domain.Utils.Helpers
{
    public static partial class StringHelper
    {
        [GeneratedRegex(@"[a-zA-Z]")]
        private static partial Regex cpfPattern();
        [GeneratedRegex(@"[a-zA-Z]")]
        private static partial Regex cnpjPattern();
        [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        private static partial Regex emailPattern();

        public static string ToSafeValue(this string? stringRequest) => stringRequest ?? string.Empty;

        public static int ToInt(this string stringRequest, string exceptionMessage)
        {
            try
            {
                return int.Parse(stringRequest);
            }
            catch
            {
                throw new FormatException(exceptionMessage);
            }
        }

        public static string ToDocument(this string stringRequest)
        {
            if (stringRequest.Length == 11)
                return stringRequest.ToCpf();
            else if (stringRequest.Length == 14)
                return stringRequest.ToCnpj();

            throw new ArgumentException("InvalidDocument");
        }

        public static string ToCpf(this string stringRequest)
        {
            if (stringRequest.Length != 11)
                throw new ArgumentException("Invalid CPF");

            var cpfPattern = @"(\d{3})(\d{3})(\d{3})(\d{2})";
            var replacement = "$1.$2.$3-$4";

            return Regex.Replace(stringRequest, cpfPattern, replacement);
        }

        public static string ToCnpj(this string stringRequest)
        {
            if (stringRequest.Length != 14)
                throw new ArgumentException("InvalidCnpj");

            var cnpjPattern = @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})";
            var replacement = "$1.$2.$3/$4-$5";

            return Regex.Replace(stringRequest, cnpjPattern, replacement);
        }

        public static bool IsValidEmail(this string stringRequest) => !stringRequest.IsNullOrEmpty() && emailPattern().IsMatch(stringRequest) && stringRequest.Length <= 89;

        public static string ToCleanJwtToken(this string stringRequest) => stringRequest.Replace("Bearer ", string.Empty);

        public static bool IsValidCpf(this string stringRequest)
        {
            if (stringRequest.Length != 11 || cpfPattern().Matches(stringRequest).Count > 0 || stringRequest.Distinct().Count() == 1)
                return false;

            stringRequest = stringRequest.Trim();

            var tempCpf = stringRequest[..9];

            var sum = 0;
            for (var i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * (10 - i);

            var remainder = sum % 11;
            var digit1 = remainder < 2 ? 0 : 11 - remainder;

            tempCpf += digit1;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * (11 - i);

            remainder = sum % 11;
            var digit2 = remainder < 2 ? 0 : 11 - remainder;

            tempCpf += digit2;

            return stringRequest.EndsWith(tempCpf);
        }
    }
}
