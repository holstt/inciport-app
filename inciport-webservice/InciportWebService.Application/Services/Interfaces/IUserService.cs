using InciportWebService.Domain;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public interface IUserService {

    Task<ApplicationUser> CreateUserAsync(UserRegistrationDto userRegistrationDto, int municipalityId, ClaimsPrincipal userClaims);

    Task<ApplicationUser> GetUserAsync(int municipalityId, string userId);

    Task<List<ApplicationUser>> GetUsersAsync(int municipalityId);

    Task DeleteUserAsync(int municipalityId, string userId);

    Task ChangePasswordAsync(int municipalityId, UserPasswordUpdateDto userPasswordUpdateDto);

    Task<ApplicationUser> UpdateUserAsync(int municipalityId, UserUpdateDto userUpdate, ClaimsPrincipal claims);
  }
}