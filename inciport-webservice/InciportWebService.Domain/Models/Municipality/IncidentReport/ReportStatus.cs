using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InciportWebService.Domain {

  public enum ReportStatus {
    Recieved = 0,
    InProgress = 1,
    Rejected = 2,
    Completed = 3,
    Archived = 4
  }
}