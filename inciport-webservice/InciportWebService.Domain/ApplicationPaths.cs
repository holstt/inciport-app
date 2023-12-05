using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InciportWebService.Domain {

  public static class ApplicationPaths {
    public static string ExecutingDirectory { get; } = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

    public const string IMAGE_DIRECTORY_NAME = "images";
    public static string ImageDirectory { get; } = Path.Combine(ExecutingDirectory, IMAGE_DIRECTORY_NAME);

    public static string GetFullPath(params string[] relativePaths) {
      relativePaths.ToList().Insert(0, ImageDirectory);
      return Path.Combine(relativePaths);
    }
  }
}