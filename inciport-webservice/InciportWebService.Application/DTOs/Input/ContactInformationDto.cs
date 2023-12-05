using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class ContactInformationDto {
    [StringLength(256)]
    public string Name { get; set; }
    [StringLength(maximumLength: 8, MinimumLength = 8)]
    public string PhoneNumber { get; set; }
    [StringLength(256), EmailAddress]
    public string Email { get; set; }

    public ContactInformation ToModel() => ToModel(existingId: 0);

    public ContactInformation ToModel(int existingId) {
      return new ContactInformation {
        Id = existingId,
        Name = Name,
        PhoneNumber = PhoneNumber,
        Email = Email
      };
    }

    public static ContactInformationDto FromModel(ContactInformation model) {
      if (model is null) {
        return null;
      }
      return new ContactInformationDto {
        Email = model.Email,
        Name = model.Name,
        PhoneNumber = model.PhoneNumber,
      };
    }
  }
}