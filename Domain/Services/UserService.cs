using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Objects.Requests.User;
using Domain.Objects.Responses.Asset;
using Domain.Utils.Helpers;
using System.Text;

namespace Domain.Services
{
    public class UserService(IAuthService authService, IEncryptionService encryptionService, IUserRepository userRepository, IMapper mapper) : IUserService
    {
        public async Task<IEnumerable<UserResultsResponse>?> Get(int currentPage, string? userName) => await userRepository.GetUserResults(currentPage, userName);

        public async Task Save(SaveUserRequest saveUserRequest)
        {
            if (await userRepository.HasUserWithSameEmail(saveUserRequest.Email))
                throw new InvalidOperationException("Email já cadastrado");

            if (await userRepository.HasUserWithSameDocument(saveUserRequest.Document))
                throw new InvalidOperationException("CPF já cadastrado");

            var user = mapper.Map<User>(saveUserRequest);

            SetUserPassword(user.Password, ref user);

            await userRepository.Save(user);
        }

        public async Task<LogInResponse> LogIn(LogInRequest logInRequest)
        {
            var userAuthInfo = await userRepository.GetUserAuthInfoByEmail(logInRequest.Email)
                ?? throw new InvalidOperationException("Email ou senha incorretos");

            var keyBytes = Encoding.ASCII.GetBytes(userAuthInfo.Name);
            var fullPasswordBytes = Encoding.ASCII.GetBytes(logInRequest.Password + userAuthInfo.Salt);
            var encryptedFullPassword = encryptionService.EncryptDeterministic(keyBytes, fullPasswordBytes);

            if (userAuthInfo.Password != encryptedFullPassword)
                throw new InvalidOperationException("Email ou senha incorretos");

            var jwtToken = authService.GenerateToken(userAuthInfo.UserId);

            if (jwtToken.IsNullOrEmpty())
                throw new InvalidOperationException();

            return new LogInResponse { AuthToken = jwtToken };
        }

        private static string GetSalt() => DateTime.UtcNow.Millisecond.ToString("000") + new Random().Next(100).ToString("00");

        private void SetUserPassword(string password, ref User user)
        {
            user.Salt = GetSalt();

            var keyBytes = Encoding.ASCII.GetBytes(user.Name);
            var fullPasswordBytes = Encoding.ASCII.GetBytes(password + user.Salt);

            user.Password = encryptionService.EncryptDeterministic(keyBytes, fullPasswordBytes);
        }
    }
}
