using Donations_App.Models;
using Donations_App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Donations_App.Helpers;
using Donations_App.Models;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Donations_App.Data;
using Donations_App.Dtos.UserDto;
using DonationsApp.Models;
using Donations_App.Dtos.ReturnDto;

namespace Donations_App.Services
{
    
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IMailingService _mailingService;
        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, IConfiguration configuration , ApplicationDbContext context , IMailingService mailingService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _configuration = configuration;
            _context = context;
            _mailingService = mailingService;
        }

        

        public async Task<AuthModel> LoginAsync(LoginDto login)
        {
            var authModel = new AuthModel();
            
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
            {
                authModel.Message = "Email or Password is incorrect !".ToString();
                return authModel;
            }
            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);
            var CartUser = await _context.Carts.SingleOrDefaultAsync(c => c.UserId == user.Id);
            authModel.Message = "User Login successfully ";
            authModel.IsAuthenticated = true;
            authModel.Id = user.Id;
            authModel.FullName = user.FullName;
             
            authModel.CartId = CartUser.Id;
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.ExpireOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();
            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return authModel;

        }

        public async Task<AuthModel> RegisterAsync(RegisterDto register )
        {
            if (await _userManager.FindByEmailAsync(register.Email) != null)
                return new AuthModel { Message = " Email is already registered !!" };
            if (await _userManager.FindByEmailAsync(register.Username) != null)
                return new AuthModel { Message = " Username is already registered !!" };
            var user = new ApplicationUser
            {
                Email = register.Email,
                FullName = register.FullName,
                UserName = register.Username,
                Address = register.Address,
                PhoneNumber = register.PhoneNumber,

            };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description} , ";
                }
                return new AuthModel { Message = errors };
            }
            await _userManager.AddToRoleAsync(user, "User");
            

            var jwtSecurityToken = await CreateJwtToken(user);
            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);
            var addCart = new Cart
            {
                UserId = user.Id,
            };
            await _context.Carts.AddAsync(addCart);
            _context.SaveChanges();

            var filePath = $"{Directory.GetCurrentDirectory()}\\Templates\\EmailTemplate.html";
            var str = new StreamReader(filePath);
            var mailText = str.ReadToEnd();
            str.Close();
            mailText = mailText.Replace("[username]", user.FullName).Replace("[email]", user.Email);
            await _mailingService.SendEmailAsync(user.Email, "Welcome to our website ", mailText);


            return new AuthModel
            {
                Message = "User Registered successfully ",
                Id = user.Id,
                Email = user.Email,
               FullName = user.FullName,   
                Username = user.UserName,
                CartId = user.Cart.Id,
                ExpireOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = refreshToken.Token ,
                RefreshTokenExpiration = refreshToken.ExpiresOn

            };
        }




        // Profile Settings -----------------------------------------------------------------------------------------------------------
        
        public async Task<AuthModel> ChangePassword(string email ,ChangePasswordDto model)
        {
            var authModel = new AuthModel();
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null || !await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
            {
                authModel.Message = "Email or Current Password is incorrect !";
                authModel.IsAuthenticated = false;
                return authModel;
            }
            var result = await _userManager.ChangePasswordAsync(user , model.CurrentPassword , model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description} , ";
                }
                return new AuthModel { Message = errors , IsAuthenticated = false };
              
            }
            var jwtSecurityToken = await CreateJwtToken(user);
            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);
            authModel.Message = "Password  Successfully Changed";
            authModel.Id = user.Id;
            authModel.FullName = user.FullName; 
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.IsAuthenticated = true;

            authModel.ExpireOn = jwtSecurityToken.ValidTo;

            authModel.Roles = new List<string> { "User" };
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.RefreshToken = refreshToken.Token;
            authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
            return authModel;

        }
        public async Task<UpdateProfileDto> GetProfileData(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            return new UpdateProfileDto
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
            };
        }

        public async Task<AuthModel> UpdateProfile(string email, UpdateProfileDto upProfile)
        {
            var authModel = new AuthModel();
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                user.FullName = upProfile.FullName;
                user.PhoneNumber = upProfile.PhoneNumber;
                user.Address = upProfile.Address;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    var errors = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        errors += $"{error.Description} , ";
                    }
                    return new AuthModel { Message = errors, IsAuthenticated = false };

                }
                var jwtSecurityToken = await CreateJwtToken(user);
                var refreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
                authModel.Message = "Profile Updated  Successfully ";
                authModel.Id = user.Id;
                authModel.FullName = user.FullName;
                authModel.Email = user.Email;
                authModel.Username = user.UserName;
                authModel.IsAuthenticated = true;

                authModel.ExpireOn = jwtSecurityToken.ValidTo;

                authModel.Roles = new List<string> { "User" };
                authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                return authModel;

            }
            return null;
        }

        public async Task<GetUserDto> GetUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new GetUserDto { isNotNull = false };
            return new GetUserDto
            {
                isNotNull = true,
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
            };


        }

        public async Task<RetRestPassDto> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return new RetRestPassDto
            {
                Success = false,
                Message = "Email is incorrect or not found !!",
            };


            Random rnd = new Random();
            var randomNum = (rnd.Next(100000, 999999)).ToString();
            string message = "Hi " + user.UserName+ " your verify code is " + randomNum;
            await _mailingService.SendEmailAsync(user.Email, "Verify Email", message, null);
            var Vcode = new VerifyCode
            {
                Code = randomNum,
                UserId = user.Id,
            };
            await _context.VerifyCodes.AddAsync(Vcode);
            _context.SaveChanges();
            return new RetRestPassDto
            {
                Success = true,
                Message = "Verify code sent to the email successfully !!",
            };
        }

        public async Task<RetRestPassDto> CreateNewPassword(string email, CreatePasswordDto model)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if(user != null)
            {
                var res =await _userManager.ResetPasswordAsync(user , model.RestToken ,model.NewPassword);
                if(res.Succeeded)
                {
                    return new RetRestPassDto
                    {
                        Success = true,
                        Message="Successfully Changed "
                    };
                }
                return new RetRestPassDto
                {
                    Success = false,
                    Message = "Error of change password"
                };

            }
            return new RetRestPassDto
            {
                Success = false,
                Message = "User is not found !!"
            };


        }

        public async Task<ReturnRestToken> VerifyCodeAsync(VerifyCodeDto codeDto)
        {
            var user = await _userManager.FindByEmailAsync(codeDto.email);
            if (user == null)
            {
                return new ReturnRestToken { Success= false};
            };
            var result = await _context.VerifyCodes.SingleOrDefaultAsync(r => r.UserId == user.Id);
            if (result.Code == codeDto.Code)
            {
                _context.VerifyCodes.Remove(result);
                _context.SaveChanges();

                var restToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                return new ReturnRestToken
                {
                    Success = true ,
                    RestToken = restToken
                };
            }
            return new ReturnRestToken { Success = false};
        }


        // Admin Methods -------------------------------------------------------------------------------------------------------------


        public async Task<AdminDto> CreateRole(CreateRoleDto createRole)
        {
            if(await _roleManager.RoleExistsAsync(createRole.RoleName))
            {
                return new AdminDto{
                    Success = false ,
                    Message = "The Role already exist !!"
                };
            }
            var result = await _roleManager.CreateAsync(new IdentityRole
           {

                  Name = createRole.RoleName
            });
            if (result.Succeeded)
            {
                return new AdminDto
                {
                    Success = true,
                    Message = "Successfully"
                };
            }
            return new AdminDto
            {
                Success = false,
                Message = "Sonething went wrong"
            };

        }

        public async Task<AdminDto> AssignRole(AssignRoleDto assignRole)
        {
            var userDatails = await _userManager.FindByEmailAsync(assignRole.Email);
            if (userDatails is null || !await _roleManager.RoleExistsAsync(assignRole.RoleName))
            {
                return new AdminDto
                {
                    Success = false,
                    Message = "Invalid user Email or Role"
                };
            }

            if (await _userManager.IsInRoleAsync(userDatails, assignRole.RoleName))
                return new AdminDto
                {
                    Success = false,
                    Message = "User already assigned to this role",
                };
            var result = await _userManager.AddToRoleAsync(userDatails, assignRole.RoleName);
            if (result.Succeeded)
            {
                return new AdminDto
                {
                    Success = true,
                    Message = "Successfully Assigned",
                };
            }
            return new AdminDto
            {
                Success = false,
                Message = "Sonething went wrong",
            };

        }

        public async Task<List<IdentityRole>> getRoles()
        {
            var result = await _roleManager.Roles.ToListAsync();
            return result;
        }


        // Methods Of Create Tokens-------------------------------------------------------------------------------------------------------
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(10),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }

        public async Task<AuthModel> RefreshTokenAsync(string token)
        {
            var authModel = new AuthModel();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                authModel.Message = "Invalid token";
                return authModel;
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                authModel.Message = "Inactive token";
                return authModel;
            }

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtToken = await CreateJwtToken(user);
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            var roles = await _userManager.GetRolesAsync(user);
            authModel.Roles = roles.ToList();
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return authModel;
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return true;
        }

    }
}