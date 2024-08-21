using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Objects.Requests.User;
using Domain.Objects.Responses.Asset;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class UserRepository(SqlContext context, IMapper mapper) : BaseSqlRepository<User>(context), IUserRepository
    {
        public async Task<User?> GetUserByEmail(string userEmail) => await _typedContext.AsNoTracking().FirstOrDefaultAsync(u => u.Email == userEmail);

        public async Task HasUserWithTheSameInfo(SaveUserRequest saveUserRequest)
        {
            if (await HasUserWithSameEmail(saveUserRequest.Email))
                throw new InvalidOperationException("UserWithSameEmailError");

            if (await HasUserWithSameDocument(saveUserRequest.Document))
                throw new InvalidOperationException("UserWithSameDocumentError");
        }

        public async Task<IEnumerable<UserResultsResponse>?> GetUserResults(int currentPage, string? userName, int takeQuantity = 10)
        {
            var query = _typedContext
                    .AsNoTracking()
                    .OrderByDescending(u => u.UserId)
                    .Skip((currentPage - 1) * takeQuantity)
                    .Take(takeQuantity);

            if (userName != null)
                query = query.Where(u => u.Name.Contains(userName));

            return await mapper.ProjectTo<UserResultsResponse>(query).ToListAsync();
        }

        private async Task<bool> HasUserWithSameEmail(string email) => await _typedContext.AsNoTracking().AnyAsync(u => u.Email == email);

        private async Task<bool> HasUserWithSameDocument(string document) => await _typedContext.AsNoTracking().AnyAsync(u => u.Document == document);
    }
}
