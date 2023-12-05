using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class ImagesService : BaseService, IImageService {
    private readonly IApplicationDbContext _dbContext;
    private readonly IFileService _fileService;

    public ImagesService(IApplicationDbContext dbContext, IFileService fileService) : base(dbContext) {
      _dbContext = dbContext;
      _fileService = fileService;
    }

    public async Task<byte[]> GetImageAsync(int municipalityId, int inciportId, int imageId) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      await EnsureInciportsExistsAsync(inciportId);
      ImageReference image = GetImageReference(municipalityId, inciportId, imageId);
      string fullImagePath = image.GetFullPath(ApplicationPaths.ExecutingDirectory);
      _fileService.EnsureFileExists(fullImagePath);
      return await _fileService.ReadAllBytesAsync(fullImagePath);
    }

    private ImageReference GetImageReference(int municipalityId, int inciportId, int imageId) {
      ImageReference image = _dbContext.Municipalities.FirstOrDefault(m => m.Id == municipalityId)
                                       .IncidentReports.FirstOrDefault(c => c.Id == inciportId)
                                       .ImageReferences.Find(i => i.Id == imageId);
      if (image is null) {
        throw new NotFoundException("Image Report", imageId);
      }

      return image;
    }
  }
}