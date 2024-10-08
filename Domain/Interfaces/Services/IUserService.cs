﻿using Domain.Objects.Requests.User;
using Domain.Objects.Responses.Asset;

namespace Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResultsResponse>?> Get(int currentPage, string? userName);
        Task Save(SaveUserRequest saveUserRequest);
        Task<LogInResponse> LogIn(LogInRequest logInRequest);
    }
}
