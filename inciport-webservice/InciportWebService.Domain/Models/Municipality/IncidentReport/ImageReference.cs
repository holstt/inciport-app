using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Domain {

  public class ImageReference {
    public int Id { get; set; }

    /// <summary>
    /// Full path of image on disk.
    /// </summary>
    public string RelativePhysicalPath { get; private set; }

    public ImageReference(string relativePhysicalPath) {
      RelativePhysicalPath = relativePhysicalPath;
    }

    public string GetFullPath(string rootPath) => Path.Combine(rootPath, RelativePhysicalPath);

    // For EF
    protected ImageReference() {
    }
  }
}