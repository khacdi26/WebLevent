using WebLevent.Areas.Identity.Data;

namespace WebLevent.Core.Repository
{
    public interface IUserRepository
    {
        ICollection<WebLeventUser> GetUsers();
        WebLeventUser GetUser(string id);
        WebLeventUser UpdateUser(WebLeventUser user);
    }
}
