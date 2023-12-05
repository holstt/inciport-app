using System.Threading;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public interface IFileService {

    void EnsureFileExists(string fullImagePath);

    Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default);
  }
}