using InciportWebService.Application;
using InciportWebService.Domain;
using InciportWebService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  [ApiController]
  public class UsersController : ApplicationControllerBase {
    private readonly IUserService _userService;

    public UsersController(IUserService userService) {
      _userService = userService;
    }

    [HttpGet]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)]
    [Route("api/municipalities/{municipalityId}/users/{userId}")]
    public async Task<IActionResult> GetUsersAsync(int municipalityId, string userId) {
      ApplicationUser user = await _userService.GetUserAsync(municipalityId, userId);
      return Ok(GetUserDto.FromModel(user));
    }

    [HttpGet]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)]
    [Route("api/municipalities/{municipalityId}/users")]
    public async Task<IActionResult> GetUsersAsync(int municipalityId) {
      List<ApplicationUser> users = await _userService.GetUsersAsync(municipalityId);
      return Ok(users.Select(u => GetUserDto.FromModel(u)).ToList());
    }

    [HttpDelete]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)]
    [Route("api/municipalities/{municipalityId}/users/{userId}")]
    public async Task<IActionResult> DeleteUserAsync(int municipalityId, string userId) {
      await _userService.DeleteUserAsync(municipalityId, userId);
      return NoContent();
    }

    [HttpPut]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)]
    [Route("api/municipalities/{municipalityId}/users/{userId}/password")]
    public async Task<IActionResult> UpdatePasswordAsync(int municipalityId, string userId, UserPasswordUpdateDto userPasswordUpdate) {
      EnsureSameId(userId, userPasswordUpdate.Id);
      await _userService.ChangePasswordAsync(municipalityId, userPasswordUpdate);
      return NoContent();
    }

    [HttpPut]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)]
    [Route("api/municipalities/{municipalityId}/users/{userId}")]
    public async Task<IActionResult> UpdateUserAsync(int municipalityId, string userId, UserUpdateDto userUpdate) {
      EnsureSameId(userId, userUpdate.Id);
      ApplicationUser updatedUser = await _userService.UpdateUserAsync(municipalityId, userUpdate, User);
      return Ok(GetUserDto.FromModel(updatedUser));
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)]
    [Route("api/municipalities/{municipalityId}/users")]
    public async Task<IActionResult> RegisterUserAsync(int municipalityId, UserRegistrationDto userRegistration) {
      ApplicationUser user = await _userService.CreateUserAsync(userRegistration, municipalityId, User);
      return Created(MakeRessourceLocationUrl(user.Id), GetUserDto.FromModel(user));
    }
  }
}