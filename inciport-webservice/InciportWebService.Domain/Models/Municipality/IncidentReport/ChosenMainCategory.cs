using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Domain {

  public class ChosenMainCategory : Category {
    public Category ChosenSubCategory { get; set; }
  }
}