using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InciportWebService.Domain {

  public class Location {
    public int Id { get; set; }
    public virtual Address Address { get; set; }
    public virtual Coordinates Coordinates { get; set; }
  }
}