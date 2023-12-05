using InciportWebService.Domain;
using InciportWebService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Api.Controllers {

  public class StatusController : ControllerBase {

    [HttpGet]
    [Route("api/statuses")]
    public IActionResult GetInciports() {
      return Ok(Enum.GetNames<ReportStatus>());
    }
  }
}