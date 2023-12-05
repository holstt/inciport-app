using InciportWebService.Application;
using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Data {

  public class ImageSaver : IImageSaver {

    /// <summary>
    /// Save a list of base64 images to disk and returns the a list of type <see cref="ImageReference"/> containing the physical file path.
    /// </summary>
    /// <param name="base64imageStrings">List of images in base 64 format</param>
    /// <returns></returns>
    public async Task<List<ImageReference>> SaveBase64ImagesAsync(List<string> base64imageStrings) {
      if (base64imageStrings is null) {
        throw new ArgumentNullException(nameof(base64imageStrings));
      }

      List<string> imagePaths = new List<string>();
      foreach (string base64imageString in base64imageStrings) {
        Guid id = Guid.NewGuid();
        string filePath = CreateRelativeImagePath(id.ToString());

        byte[] imageBytes;
        try {
          imageBytes = Convert.FromBase64String(base64imageString);
        }
        catch (FormatException) {
          throw new ValidationException("One or more images was not in valid base64 format");
        }
        await File.WriteAllBytesAsync(filePath, imageBytes);
        imagePaths.Add(filePath);
      }

      return imagePaths.Select(id => new ImageReference(id)).ToList();
    }

    public static string CreateRelativeImagePath(string imageId) => Path.Combine(ApplicationPaths.IMAGE_DIRECTORY_NAME, $"{imageId}.jpg");
  }
}