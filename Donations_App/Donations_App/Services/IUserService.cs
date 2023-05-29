using System.Threading.Tasks;
using Donations_App.Models;
using Donations_App.Dtos.UserDto;
using Microsoft.AspNetCore.Identity;
using Donations_App.Dtos.ReturnDto;
using Donations_App.Dtos.RequestDtos;

namespace Donations_App.Services
{
    public interface IUserService
    {
        Task<AuthModel> RegisterAsync(RegisterDto register);
        Task<AuthModel> LoginAsync(LoginDto login);
        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task<GetAllUsers> GetAllUsers();
        Task<UsersDetails> GetUsersDetails();
        //--------------------------------------------------------------------------

        Task<UpdateProfileDto> GetProfileData(string email);
        Task<AuthModel> UpdateProfile(string email, UpdateProfileDto upProfile);
        Task<GetUserDto> GetUser(GetUserData dto);
        Task<GeneralRetDto> ForgotPasswordAsync(string email);
        Task<RestTokenDto> VerifyCodeAsync(VerifyCodeDto codeDto);
        Task<AuthModel> ChangePassword(string email ,ChangePasswordDto model);
        Task<GeneralRetDto> CreateNewPassword(string email, CreatePasswordDto model);

        //---------------------------------------------------------------------------------------------------------------
        Task<GeneralRetDto> AssignRole(AssignRoleDto assignRole);
        Task<GeneralRetDto> CreateRole(CreateRoleDto createRole);
        Task<List<IdentityRole>> getRoles();
        
    }
}