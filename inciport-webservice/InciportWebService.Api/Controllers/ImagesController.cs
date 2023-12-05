using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using InciportWebService.Application;
using Microsoft.AspNetCore.Authorization;
using InciportWebService.Domain;
using InciportWebService.Data;
using Microsoft.Extensions.Primitives;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Identity;

namespace InciportWebService.Api {

  [ApiController]
  public class ImagesController : ApplicationControllerBase {
    private readonly IImageService _imagesService;

    public ImagesController(IImageService imagesService) {
      _imagesService = imagesService;
    }

    [HttpGet]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_INTERNAL_ACCESS)]
    [Route("api/municipalities/{municipalityId}/inciports/{inciportId}/images/{imageId}")]
    public async Task<IActionResult> GetImage(int municipalityId, int inciportId, int imageId) {
      byte[] byteImage = await _imagesService.GetImageAsync(municipalityId, inciportId, imageId);
      return File(byteImage, "image/jpeg");
    }
  }
}