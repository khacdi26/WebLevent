using WebLevent.Areas.Identity.Data;
using WebLevent.Core.Repository;

namespace WebLevent.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly WebLeventContext _context;

        public UserRepository(WebLeventContext context)
        {
            _context = context;
        }

        public WebLeventUser GetUser(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public ICollection<WebLeventUser> GetUsers()
        {
            return _context.Users.ToList();
        }

        public WebLeventUser UpdateUser(WebLeventUser user)
        {
             _context.Update(user);
            _context.SaveChanges();
            return user;
        }
    }
}
