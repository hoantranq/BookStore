using BookStore.API.DTOs;
using BookStore.API.Models;
using System.Threading.Tasks;

namespace BookStore.API.Services.Users
{
    public interface IUserService
    {
        Task<string> RegisterAsync(UserForRegisterDto userForRegisterDto);
        Task<AuthenticationResponse> LoginAsync(UserForLoginDto tokenRequest);
    }
}
