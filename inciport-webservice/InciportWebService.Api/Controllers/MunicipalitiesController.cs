using InciportWebService.Application;
using InciportWebService.Domain;
using InciportWebService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  [ApiController]
  public class MunicipalitiesController : ApplicationControllerBase {
    private readonly ApplicationDbContext _dbContext;
    private readonly IAuthorizationService _authorizationService;

    public MunicipalitiesController(ApplicationDbContext dbContext, IAuthorizationService authorizationService) {
      _dbContext = dbContext;
      _authorizationService = authorizationService;
    }

    private async Task<bool> IsSuperUserAsync() {
      return (await _authorizationService.AuthorizeAsync(User, AuthorizationPolicyNames.REQUIRE_SUPER_USER)).Succeeded;
    }

    [HttpGet]
    [Route("api/municipalities/{id}")]
    public async Task<IActionResult> GetMunicipality(int id) {
      MunicipalityEntity entity = (await _dbContext.Municipalities.FindAsync(id));
      if (entity is null) {
        return NotFound("Municipality", id);
      }

      if (await IsSuperUserAsync()) {
        MunicipalityDto dto = MunicipalityDto.FromEntity(entity);
        return Ok(dto);
      }
      else {
        MunicipalityUnauthorizedDto dtoUnauthorized = MunicipalityUnauthorizedDto.FromEntity(entity);
        return Ok(dtoUnauthorized);
      }
    }

    [HttpGet]
    [Route("api/municipalities")]
    public async Task<IActionResult> GetMunicipalities() {
      if (await IsSuperUserAsync()) {
        List<MunicipalityDto> dto = (await _dbContext.Municipalities.ToListAsync()).Select(m => MunicipalityDto.FromEntity(m)).ToList();
        return Ok(dto);
      }
      else {
        List<MunicipalityUnauthorizedDto> dto = (await _dbContext.Municipalities.ToListAsync()).Select(m => MunicipalityUnauthorizedDto.FromEntity(m)).ToList();
        return Ok(dto);
      }
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_SUPER_USER)]
    [Route("api/municipalities")]
    public async Task<IActionResult> CreateMunicipality(CreateMunicipalityDto inputDto) {
      MunicipalityEntity entity = new MunicipalityEntity {
        Name = inputDto.Name
      };

      bool isDuplicateName = await _dbContext.Municipalities.AnyAsync(m => m.Name.ToLower() == inputDto.Name.ToLower());
      if (isDuplicateName) {
        return BadRequest($"Municipality with name {inputDto.Name} already exists");
      }

      _dbContext.Municipalities.Add(entity);
      await _dbContext.SaveChangesAsync();

      return Created(MakeRessourceLocationUrl(entity.Id), MunicipalityDto.FromEntity(entity));
    }

    [HttpPut]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_SUPER_USER)]
    [Route("api/municipalities/{id}")]
    public async Task<IActionResult> UpdateMunicipality(int id, MunicipalityUnauthorizedDto inputDto) {
      if (id != inputDto.Id) {
        return BadRequest($"Endpoint id {id} was different from object id {inputDto.Id}");
      }

      MunicipalityEntity entity = (await _dbContext.Municipalities.FindAsync(id));
      if (entity is null) {
        return NotFound("Municipality", id);
      }

      entity.Name = inputDto.Name;
      await _dbContext.SaveChangesAsync();

      return Ok(MunicipalityDto.FromEntity(entity));
    }

    [HttpDelete]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_SUPER_USER)]
    [Route("api/municipalities/{id}")]
    public async Task<IActionResult> UpdateMunicipality(int id) {
      MunicipalityEntity entity = (await _dbContext.Municipalities.FindAsync(id));
      if (entity is null) {
        return NotFound("Municipality", id);
      }

      _dbContext.Remove(entity);
      await _dbContext.SaveChangesAsync();

      return NoContent();
    }
  }
}