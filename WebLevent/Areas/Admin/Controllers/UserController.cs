using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using WebLevent.Areas.Identity.Data;
using WebLevent.Core.Repository;
using WebLevent.Core.ViewModel;

namespace WebLevent.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<WebLeventUser> _signInManager;
        public INotyfService _notifyService { get; }


        public UserController(IUnitOfWork unitOfWork, SignInManager<WebLeventUser> signInManager, INotyfService notyfService)
        {
            _signInManager=signInManager;
            _unitOfWork = unitOfWork;
            _notifyService = notyfService;
        }
        public IActionResult Index()
        {
            var userList = _unitOfWork.User.GetUsers();
            return View(userList);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var user = _unitOfWork.User.GetUser(id);
            var roles = _unitOfWork.Role.GetRoles();
            var userRoles =  await _signInManager.UserManager.GetRolesAsync(user);

            var roleItems = roles.Select(role => 
                new SelectListItem(
                    role.Name,
                    role.Id, 
                    userRoles.Any(ur => ur.Contains(role.Name))
            )).ToList();

            var vm = new EditUserViewModel
            {
                User = user,
                Roles = roleItems
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(EditUserViewModel data)
        {

            var user = _unitOfWork.User.GetUser(data.User.Id);
            if (user == null)
            {
                return NotFound();
            }

            var userRolesInDb = await _signInManager.UserManager.GetRolesAsync(user);

            var rolesToAdd = new List<string>();
            var rolesToRemove = new List<string>();

            foreach (var role in data.Roles)
            {
                var assignedInDb = userRolesInDb.FirstOrDefault(ur => ur == role.Text);
                if (role.Selected)
                {
                    if (assignedInDb == null)
                    {
                        rolesToAdd.Add(role.Text);
                    }
                }
                else
                {
                    if (assignedInDb != null)
                    {
                        rolesToRemove.Add(role.Text);
                    }
                }
            }
            if (rolesToAdd.Any())
            {
                await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);
            }
            if (rolesToRemove.Any())
            {
                await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            user.FullName = data.User.FullName;
            user.Email = data.User.Email;

            _unitOfWork.User.UpdateUser(user);
            _notifyService.Success("Cập nhật người dùng thành công!");

            return RedirectToAction(nameof(Index));
        }
    }
}