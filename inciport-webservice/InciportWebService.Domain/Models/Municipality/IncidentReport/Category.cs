using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Domain {

  public class Category {
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsArchived { get; set; }
  }
}