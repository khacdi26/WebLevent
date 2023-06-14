using Microsoft.AspNetCore.Identity;
using WebLevent.Areas.Identity.Data;
using WebLevent.Core.Repository;

namespace WebLevent.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private WebLeventContext _context;

        public RoleRepository(WebLeventContext context)
        {
            _context = context;
        }
        public ICollection<IdentityRole> GetRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
