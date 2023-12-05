using InciportWebService.Application;
using InciportWebService.Domain;
using InciportWebService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
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
  public class WorkerTeamsController : ApplicationControllerBase {
    private readonly ApplicationDbContext _dbContext;
    private readonly IWorkerTeamsService _workerTeamsService;

    public WorkerTeamsController(ApplicationDbContext dbContext, IWorkerTeamsService workerTeamsService) {
      _dbContext = dbContext;
      _workerTeamsService = workerTeamsService;
    }

    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_INTERNAL_ACCESS)]
    [HttpGet]
    [Route("api/municipalities/{municipalityId}/workerteams")]
    public async Task<IActionResult> GetWorkerTeams(int municipalityId) {
      List<WorkerTeam> workerTeams = await _workerTeamsService.GetWorkerTeams(municipalityId);
      return Ok(workerTeams.Select(w => GetWorkerTeamDto.FromModel(w)));
    }

    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_INTERNAL_ACCESS)]
    [HttpGet]
    [Route("api/municipalities/{municipalityId}/workerteams/{workerTeamId}")]
    public async Task<IActionResult> GetWorkerTeam(int municipalityId, int workerTeamId) {
      WorkerTeam workerTeam = await _workerTeamsService.GetWorkerTeam(municipalityId, workerTeamId);
      return Ok(GetWorkerTeamDto.FromModel(workerTeam));
    }

    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)]
    [HttpPost]
    [Route("api/municipalities/{municipalityId}/workerteams")]
    public async Task<IActionResult> CreateWorkerTeam(int municipalityId, CreateWorkerTeamDto inputDto) {
      var createdTeam = await _workerTeamsService.CreateWorkerTeams(municipalityId, inputDto);
      return Created(MakeRessourceLocationUrl(createdTeam.Id), new GetWorkerTeamDto { Id = createdTeam.Id, Name = createdTeam.Name });
    }

    [HttpPut]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)]
    [Route("api/municipalities/{municipalityId}/workerteams/{workerTeamId}")]
    public async Task<IActionResult> UpdateWorkerTeam(int municipalityId, int workerTeamId, GetWorkerTeamDto inputDto) {
      if (workerTeamId != inputDto.Id) {
        return BadRequest($"Endpoint id {workerTeamId} was different from object id {inputDto.Id}");
      }

      MunicipalityEntity municipality = await _dbContext.Municipalities.FindAsync(municipalityId);
      if (municipality is null) {
        return NotFound("Municipality", municipalityId);
      }

      WorkerTeam workerTeam = municipality.WorkerTeams.Find(r => r.Id == workerTeamId && !r.IsArchived);

      if (workerTeam is null) {
        return NotFound("Worker team", workerTeamId);
      }

      workerTeam.Name = inputDto.Name;
      await _dbContext.SaveChangesAsync();
      return Ok(inputDto);
    }

    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)]
    [HttpDelete]
    [Route("api/municipalities/{municipalityId}/workerteams/{workerTeamId}")]
    public async Task<IActionResult> DeleteWorkerTeam(int municipalityId, int workerTeamId) {
      MunicipalityEntity municipality = await _dbContext.Municipalities.FindAsync(municipalityId);
      if (municipality is null) {
        return NotFound("Municipality", municipalityId);
      }

      WorkerTeam model = municipality.WorkerTeams.Find(r => r.Id == workerTeamId && !r.IsArchived);

      if (model is null) {
        return NotFound("Worker team", workerTeamId);
      }

      model.IsArchived = true;
      await _dbContext.SaveChangesAsync();
      return NoContent();
    }
  }
}