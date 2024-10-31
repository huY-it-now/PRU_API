using Application.ViewModels;
using Domain.Contracts.Abstracts.Shared;

namespace Application.Interfaces
{
    public interface IPlayerService
    {
        Task<Result<object>> CreateAccount(RegisterUserDTO req);
    }
}
