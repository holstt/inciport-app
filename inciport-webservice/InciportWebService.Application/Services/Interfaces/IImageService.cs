using System.Threading.Tasks;

namespace InciportWebService.Application {

  public interface IImageService {
    Task<byte[]> GetImageAsync(int municipalityId, int inciportId, int imageId);
  }
}