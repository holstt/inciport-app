using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public interface IIncidentReportService {

    Task<IncidentReport> GetIncidentReportAsync(int municipalityId, int inciportId);

    Task<List<IncidentReport>> GetIncidentReportsAsync(int municipalityId);

    Task DeleteIncidentReportAsync(int municipalityId, int inciportId);

    Task<IncidentReport> CreateIncidentReportAsync(int municipalityId, CreateIncidentReportDto reportToCreate);

    Task<IncidentReport> UpdateIncidentReportAsync(int municipalityId, int inciportId, UpdateIncidentReportDto reportToUpdate);
  }
}