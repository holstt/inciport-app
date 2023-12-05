using InciportWebService.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using InciportWebService.Application;

namespace InciportWebService.Api
{

    [ApiController]
  public class IncidentReportsController : ApplicationControllerBase {
    private readonly IIncidentReportService _incidentReportService;

    public IncidentReportsController(IIncidentReportService incidentReportService) {
      _incidentReportService = incidentReportService;
    }

    [HttpGet]
    [Route("api/municipalities/{municipalityId}/inciports/{inciportId}")]
    public async Task<IActionResult> GetIncidentReport(int municipalityId, int inciportId) {
      IncidentReport incidentReportModel = await _incidentReportService.GetIncidentReportAsync(municipalityId, inciportId);
      GetIncidentReportDto dto = GetIncidentReportDto.FromModel(incidentReportModel, new Uri(GetRequestBaseUrl(), $"api/municipalities/{municipalityId}/inciports/{inciportId}/"));
      return Ok(dto);
    }

    [HttpGet]
    [Route("api/municipalities/{municipalityId}/inciports")]
    public async Task<IActionResult> GetIncidentReports(int municipalityId) {
      List<IncidentReport> incidentReportModel = await _incidentReportService.GetIncidentReportsAsync(municipalityId);
      List<GetIncidentReportDto> dtos = incidentReportModel.Select(i => GetIncidentReportDto.FromModel(i, new Uri(GetRequestBaseUrl(), $"api/municipalities/{municipalityId}/inciports/{i.Id}/"))).ToList();
      return Ok(dtos);
    }

    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_INTERNAL_ACCESS)]
    [HttpDelete]
    [Route("api/municipalities/{municipalityId}/inciports/{inciportId}")]
    public async Task<IActionResult> DeleteIncidentReport(int municipalityId, int inciportId) {
      await _incidentReportService.DeleteIncidentReportAsync(municipalityId, inciportId);
      return NoContent();
    }

    [HttpPost]
    [Route("api/municipalities/{municipalityId}/inciports")]
    public async Task<IActionResult> CreateIncidentReport(int municipalityId, CreateIncidentReportDto reportToCreate) {
      IncidentReport incidentReport = await _incidentReportService.CreateIncidentReportAsync(municipalityId, reportToCreate);
      GetIncidentReportDto createdIncidentReportDto = GetIncidentReportDto.FromModel(incidentReport, GetFullRequestUrl());
      return Created(MakeRessourceLocationUrl(createdIncidentReportDto.Id), createdIncidentReportDto);
    }

    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_INTERNAL_ACCESS)]
    [HttpPut]
    [Route("api/municipalities/{municipalityId}/inciports/{inciportId}")]
    public async Task<IActionResult> UpdateIncidentReport(int municipalityId, int inciportId, UpdateIncidentReportDto reportToUpdate) {
      EnsureSameId(inciportId, reportToUpdate.Id);
      IncidentReport incidentReport = await _incidentReportService.UpdateIncidentReportAsync(municipalityId, inciportId, reportToUpdate);
      GetIncidentReportDto createdIncidentReportDto = GetIncidentReportDto.FromModel(incidentReport, GetFullRequestUrl());
      return Ok(createdIncidentReportDto);
    }
  }
}