using Ergo.Application.Contracts.Identity;
using Ergo.Application.Models.Identity;
using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using Ergo.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ergo.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRepository userRepository;
        private readonly IBadgeRepository badgeRepository;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly IPasswordResetCode passwordResetCodeRepository;
        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, IUserRepository userRepository, IPasswordResetCode passwordResetCodeRepository, IBadgeRepository badgeRepository)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.userRepository = userRepository;
            this.passwordResetCodeRepository = passwordResetCodeRepository;
            this.badgeRepository = badgeRepository;
        }
        public async Task<(int, string)> Registeration(RegistrationModel model, string role)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return (0, "User already exists");
            var userExistsByEmail = await userManager.FindByEmailAsync(model.Email);
            if (userExistsByEmail != null)
                return (0, "User with this email already exists");
            if (!IsPasswordValid(model.Password))
                return (0, "Password is not valid! The password must have at least 7 characters and needs to include a capital letter, a symbol, a digit.");
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Name = model.Name
            };
            var createUserResult = await userManager.CreateAsync(user, model.Password);
            
            
            if (!createUserResult.Succeeded)
            { 
                return (0, "User creation failed! Please check user details and try again.");
            }

            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

            if (await roleManager.RoleExistsAsync(UserRoles.User))
                await userManager.AddToRoleAsync(user, role);
            var userDomain = User.Create(Guid.Parse(user.Id));
            await userRepository.AddAsync(userDomain.Value);
            var badgeInnovator = Badge.Create(BadgeConstants.InnovatorBadgeName,0,Guid.Parse(user.Id),BadgeConstants.InnovatorBadgeType);
            await badgeRepository.AddAsync(badgeInnovator.Value);
            var badgeQualityMaster = Badge.Create(BadgeConstants.QualityMasterBadgeName,0,Guid.Parse(user.Id),BadgeConstants.QualityMasterBadgeType);
            await badgeRepository.AddAsync(badgeQualityMaster.Value);
            var badgeProblemSolver = Badge.Create(BadgeConstants.ProblemSolverBadgeName,0,Guid.Parse(user.Id),BadgeConstants.ProblemSolverBadgeType);
            await badgeRepository.AddAsync(badgeProblemSolver.Value);
            var badgeTeamPlayer = Badge.Create(BadgeConstants.TeamPlayerBadgeName,0,Guid.Parse(user.Id),BadgeConstants.TeamPlayerBadgeType);
            await badgeRepository.AddAsync(badgeTeamPlayer.Value);
            return (1, "User created successfully!");
        }

        public async Task<(int, string)> Login(LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username!);
            if (user == null)
                return (0, "Invalid username");
            if (!await userManager.CheckPasswordAsync(user, model.Password!))
                return (0, "Invalid password");

            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName!),
               new Claim(ClaimTypes.Email, user.Email!),
               new Claim(ClaimTypes.NameIdentifier, user.Id!),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            string token = GenerateToken(authClaims);
            return (1, token);
        }
        public async Task<(int, string)> ResetPassword(ResetPasswordModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return (0, "User with the provided email does not exist.");
            var resetCodeValid = await passwordResetCodeRepository.HasValidCodeByEmailAsync(model.Email, model.Code);
            if (!resetCodeValid)
                return (0, "Invalid reset code.");
            var codeHash = userManager.PasswordHasher.HashPassword(user, model.Password);
            user.PasswordHash = codeHash;
            var updateResult = await userManager.UpdateAsync(user);
            await passwordResetCodeRepository.InvalidateExistingCodesAsync(model.Email);

            if (!updateResult.Succeeded)
            {
                return (0, "Password reset failed! Please check user details and try again.");
            }

            await userManager.UpdateSecurityStampAsync(user);

            return (1, "Password reset successfully!");

        }

        public async Task<(int, string)> Logout()
        {
            await signInManager.SignOutAsync();
            return (1, "User logged out successfully!");
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = configuration["JWT:ValidIssuer"]!,
                Audience = configuration["JWT:ValidAudience"]!,
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private bool IsPasswordValid(string password)
        {
            var passwordValidator = new PasswordValidator<ApplicationUser>();
            var result = passwordValidator.ValidateAsync(userManager, null, password);
            return result.Result.Succeeded;
        }
    }
}
