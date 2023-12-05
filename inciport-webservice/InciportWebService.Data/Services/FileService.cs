using InciportWebService.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InciportWebService.Data {

  public class FileService : IFileService {

    public void EnsureFileExists(string fullImagePath) {
      if (!System.IO.File.Exists(fullImagePath)) {
        // Case: We stored an image with an invalid path. This should not happen.
        throw new Exception($"Expected image stored on path '{fullImagePath }' but none was found");
      }
    }

    public async Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default) => await System.IO.File.ReadAllBytesAsync(path);
  }
}