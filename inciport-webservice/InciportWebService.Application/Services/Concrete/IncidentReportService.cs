using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class IncidentReportService : BaseService, IIncidentReportService {
    private readonly ICategoriesService _categoriesService;
    private readonly IWorkerTeamsService _workerTeamsService;
    private readonly IImageSaver _imageSaver;

    public IncidentReportService(IApplicationDbContext dbContext,
                                 ICategoriesService categoriesService,
                                 IWorkerTeamsService workerTeamsService,
                                 IImageSaver imageSaver) : base(dbContext) {
      _categoriesService = categoriesService;
      _workerTeamsService = workerTeamsService;
      _imageSaver = imageSaver;
    }

    public async Task<IncidentReport> GetIncidentReportAsync(int municipalityId, int inciportId) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      IncidentReportEntity incidentReportEntity = await GetEntity(municipalityId, inciportId);
      return await EntityToModelAsync(incidentReportEntity);
    }

    public async Task<List<IncidentReport>> GetIncidentReportsAsync(int municipalityId) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      List<IncidentReportEntity> entities = await GetEntities(municipalityId);
      return await EntityToModelAsync(entities);
    }

    public async Task DeleteIncidentReportAsync(int municipalityId, int inciportId) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      IncidentReportEntity entity = await GetEntity(municipalityId, inciportId);
      DbContext.IncidentReportEntities.Remove(entity);
      await DbContext.SaveChangesAsync();
    }

    public async Task<IncidentReport> CreateIncidentReportAsync(int municipalityId, CreateIncidentReportDto reportToCreate) {
      await EnsureMunicipalityExistsAsync(municipalityId);

      MainCategory mainCategory = await _categoriesService.GetCategoryAsync(municipalityId, reportToCreate.ChosenMainCategory.Id.Value);
      Category subCategory = GetSubcategory(reportToCreate.ChosenMainCategory.ChosenSubCategory, mainCategory);

      IncidentReport model = new IncidentReport {
        Status = ReportStatus.Recieved,
        TimestampCreatedUtc = DateTimeOffset.UtcNow,
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow,
        AssignedTeam = null, // User cannot assign team at creation
        ChosenMainCategory = mainCategory.ToChosenMainCategory(selectedSubCategoryId: subCategory?.Id),
        Location = reportToCreate.Location.ToModel(),
        Description = reportToCreate.Description,
        ContactInformation = GetIfAnyContactInformation(reportToCreate.ContactInformation),
        ImageReferences = await SaveImages(reportToCreate.ImagesBase64)
      };

      IncidentReportEntity entity = IncidentReportEntity.FromModel(model);
      await SaveIncidentReport(municipalityId, entity);
      model.Id = entity.Id; // Got id after save.
      return model;
    }

    public async Task<IncidentReport> UpdateIncidentReportAsync(int municipalityId, int inciportId, UpdateIncidentReportDto reportToUpdate) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      IncidentReportEntity entityExisting = await GetEntity(municipalityId, inciportId);

      MainCategory mainCategory = await _categoriesService.GetCategoryAsync(municipalityId, reportToUpdate.ChosenMainCategory.Id.Value);
      Category subCategory = GetSubcategory(reportToUpdate.ChosenMainCategory.ChosenSubCategory, mainCategory);

      WorkerTeam team = null;
      if (reportToUpdate.AssignedTeam != null) {
        team = await _workerTeamsService.GetWorkerTeam(municipalityId, reportToUpdate.AssignedTeam.Id.Value);
      }

      IncidentReport modelUpdated = new IncidentReport {
        Id = reportToUpdate.Id,
        Status = reportToUpdate.Status,
        TimestampCreatedUtc = entityExisting.TimestampCreatedUtc,
        TimestampLastModifiedUtc = DateTimeOffset.UtcNow,
        AssignedTeam = team,
        ChosenMainCategory = mainCategory.ToChosenMainCategory(selectedSubCategoryId: subCategory?.Id),
        Location = reportToUpdate.Location.ToModel(entityExisting.Location.Id, entityExisting.Location.Address.Id),
        Description = reportToUpdate.Description,
        ContactInformation = GetContactInformation(reportToUpdate.ContactInformation, entityExisting.ContactInformation),
        ImageReferences = entityExisting.ImageReferences
      };

      IncidentReportEntity entityUpdated = IncidentReportEntity.FromModel(modelUpdated);
      DbContext.ClearTracker(); // Do not track existing entity to avoid conflicts.
      DbContext.Update(entityUpdated);
      await DbContext.SaveChangesAsync();
      return modelUpdated;
    }

    private static ContactInformation GetIfAnyContactInformation(ContactInformationDto contactInformation) {
      return contactInformation is null ? null : contactInformation.ToModel();
    }

    private static ContactInformation GetContactInformation(ContactInformationDto updatedContactInformation, ContactInformation existingContactInformation) {
      // No contact information specified.
      if (updatedContactInformation is null) {
        return null;
      }

      if (existingContactInformation is not null) {
        // Has existing contact information. Use this id to update the existing entity.
        return updatedContactInformation.ToModel(existingContactInformation.Id);
      }
      else {
        return updatedContactInformation.ToModel();
      }
    }

    private static Category GetSubcategory(ChosenSubCategoryDto chosenSubCategoryDto, MainCategory mainCategory) {
      Category subCategory = null;
      // If submitted main category has no sub category.
      if (chosenSubCategoryDto is null && mainCategory.SubCategories.Count > 0) {
        throw new ValidationException($"A sub category is required for main category {mainCategory.Id} (id), {mainCategory.Title} (title), but none was provided.");
      }
      else if (chosenSubCategoryDto is not null) {
        // Case: If submitted sub category does not exist for this main category.
        // Find sub category in main category
        subCategory = mainCategory.SubCategories.FirstOrDefault(c => c.Id == chosenSubCategoryDto.Id);
        if (subCategory is null) {
          throw new NotFoundException("Sub category", chosenSubCategoryDto.Id.Value);
        }
      }

      return subCategory;
    }

    private async Task SaveIncidentReport(int municipalityId, IncidentReportEntity modelCreated) {
      DbContext.Municipalities.FirstOrDefault(m => m.Id == municipalityId).IncidentReports.Add(modelCreated);
      await DbContext.SaveChangesAsync();
    }

    private async Task<List<ImageReference>> SaveImages(List<string> imagesBase64) {
      if (imagesBase64 is null) {
        return new List<ImageReference>();
      }
      return await _imageSaver.SaveBase64ImagesAsync(imagesBase64);
    }

    private async Task<List<IncidentReport>> EntityToModelAsync(List<IncidentReportEntity> entities) {
      // Convert entities to model
      List<IncidentReport> models = new List<IncidentReport>();
      foreach (IncidentReportEntity entity in entities) {
        IncidentReport model = await EntityToModelAsync(entity);
        models.Add(model);
      }

      return models;
    }

    private async Task<List<IncidentReportEntity>> GetEntities(int municipalityId) {
      List<IncidentReportEntity> incidentReportEntity = (await DbContext.Municipalities.Where(m => m.Id == municipalityId)
                                                          .Select(m => m.IncidentReports.Where(i => i.Status != ReportStatus.Archived)).FirstOrDefaultAsync())
                                                          .ToList();
      return incidentReportEntity;
    }

    private async Task<IncidentReportEntity> GetEntity(int municipalityId, int inciportId) {
      IncidentReportEntity incidentReportEntity = (await DbContext.Municipalities.Where(m => m.Id == municipalityId)
                                                                .Select(m => m.IncidentReports.Where(i => i.Id == inciportId && i.Status != ReportStatus.Archived)).FirstOrDefaultAsync())
                                                                .FirstOrDefault();
      if (incidentReportEntity is null) {
        throw new NotFoundException("Incident Report", inciportId);
      }

      return incidentReportEntity;
    }

    public async Task<IncidentReport> EntityToModelAsync(IncidentReportEntity entity) {
      // Find category match from db
      MainCategory mainCategoryMatch = await DbContext.MainCategories.FindAsync(entity.ChosenMainCategoryEntity.MainCategoryId);
      if (mainCategoryMatch is null) {
        throw new Exception("Existing incident report is refering a main category that does not exist");
      }

      ChosenMainCategory chosenMainCategory = mainCategoryMatch.ToChosenMainCategory(entity.ChosenMainCategoryEntity?.SubCategoryId);
      return entity.ToModel(chosenMainCategory: chosenMainCategory);
    }
  }
}