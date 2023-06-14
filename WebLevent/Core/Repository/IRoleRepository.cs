using Microsoft.AspNetCore.Identity;
using WebLevent.Areas.Identity.Data;

namespace WebLevent.Core.Repository
{
    public interface IRoleRepository
    {
        ICollection<IdentityRole> GetRoles();
    }
}
