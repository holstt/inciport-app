using InciportWebService.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public interface IImageSaver {

    Task<List<ImageReference>> SaveBase64ImagesAsync(List<string> base64imageStrings);
  }
}