using InciportWebService.Application;
using InciportWebService.Domain;
using InciportWebService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  [ApiController]
  public class LoginController : ApplicationControllerBase {
    private readonly AuthencicationService _authencicationService;

    public LoginController(AuthencicationService authencicationService) {
      _authencicationService = authencicationService;
    }

    [HttpPost]
    [Route("api/auth/login")]
    public async Task<IActionResult> LoginAsync(UserLoginRequestDto dto) {
      AuthorizedUser user = await _authencicationService.LoginAsync(dto.Email, dto.Password);
      return Ok(UserLoginResponseDto.FromModel(user));
    }
  }
}