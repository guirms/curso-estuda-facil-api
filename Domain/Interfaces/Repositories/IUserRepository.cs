using Domain.Models;
using Domain.Objects.Dto_s.User;
using Domain.Objects.Responses.Asset;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository : IBaseSqlRepository<User>
    {
        Task<UserAuthInfoDto?> GetUserAuthInfoByEmail(string email);
        Task<bool> HasUserWithSameEmail(string email);
        Task<bool> HasUserWithSameDocument(string document);
        Task<IEnumerable<UserResultsResponse>?> GetUserResults(int currentPage, string? userName, int takeQuantity = 10);
    }
}
