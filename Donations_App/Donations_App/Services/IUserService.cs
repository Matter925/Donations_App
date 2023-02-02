using System.Threading.Tasks;
using Donations_App.Models;
using Donations_App.Dtos.UserDto;
using Microsoft.AspNetCore.Identity;

namespace Donations_App.Services
{
    public interface IUserService
    {
        Task<AuthModel> RegisterAsync(RegisterDto register);
        Task<AuthModel> LoginAsync(LoginDto login);
        Task<UpdateProfileDto> GetProfileData(string email);
        Task<AuthModel> UpdateProfile(string email, UpdateProfileDto upProfile);
        Task<GetUserDto> GetUser(string email);
        Task<AuthModel> ForgotPasswordAsync(string email);
        Task<bool> VerifyCodeAsync(VerifyCodeDto codeDto);
        Task<AuthModel> ChangePassword(string email ,ChangePasswordDto model);
        Task<string> AssignRole(AssignRoleDto assignRole);
        Task<bool> CreateRole(CreateRoleDto createRole);
        Task<List<IdentityRole>> getRoles();
        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}