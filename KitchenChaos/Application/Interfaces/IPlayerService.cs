using Application.ViewModels;
using Domain.Contracts.Abstracts.Shared;

namespace Application.Interfaces
{
    public interface IPlayerService
    {
        Task<Result<object>> CreateAccount(RegisterUserDTO req);
        Task<Result<object>> SignInAccount(LoginUserDTO req);
        Task<Result<object>> Verify(VerifyTokenDTO request);
    }
}
