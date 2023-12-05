using System.Threading.Tasks;

namespace InciportWebService.Application {
  public interface IAuthencicationService {
    Task<AuthorizedUser> LoginAsync(string email, string password);
  }
}