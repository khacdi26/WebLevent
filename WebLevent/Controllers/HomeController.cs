using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PagedList.Core;
using System.Diagnostics;
using WebLevent.Areas.Identity.Data;
using WebLevent.Models;

namespace WebLevent.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, WebLeventContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
            _logger = logger;
        }
        private readonly WebLeventContext _context;
        public INotyfService _notifyService { get; }


        public IActionResult Index(int page = 1, int CatID = 0)
        {
            var pageNumber = page;
            var pageSize = 9;
            List<SanPham> lsProducts = new List<SanPham>();
            if (CatID != 0)
            {
                lsProducts = _context.SanPham
                .AsNoTracking()
                .Where(x => x.LoaiId == CatID)
                .Include(x => x.Loai)
                .OrderByDescending(x => x.Id).ToList();
            }
            else
            {
                lsProducts = _context.SanPham.AsNoTracking()
                .Include(x => x.Loai)
                .OrderByDescending(x => x.Id).ToList();
            }

            PagedList<SanPham> models = new PagedList<SanPham>(lsProducts.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentCateID = CatID;
            ViewBag.CurrentPage = pageNumber;
            ViewData["DanhMuc"] = new SelectList(_context.LoaiSanPham, "Id", "Name");
            return View(models);
        }
        public IActionResult Filtter(int CatID = 0)
        {
            var url = $"/Products?CatID={CatID}";
            if (CatID == 0)
            {
                url = $"/Products";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Contact()
        {
            ViewData["DanhMuc"] = new SelectList(_context.LoaiSanPham, "Id", "Name");

            return View();
        }
        public IActionResult About()
        {
            ViewData["DanhMuc"] = new SelectList(_context.LoaiSanPham, "Id", "Name");

            return View();
        }

        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";

        // Lấy cart từ Session (danh sách CartItem)
        List<CartItem> GetCartItems()
        {

            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItem> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }

        [Route("addcart/{id:int}", Name = "addcart")]
        public IActionResult AddToCart([FromRoute] int id)
        {

            var sanpham= _context.SanPham
                .Where(p => p.Id == id)
                .FirstOrDefault();
            if (sanpham == null)
                return NotFound("Không có sản phẩm");

            // Xử lý đưa vào Cart ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.sanPham.Id== id);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity++;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartItem() { quantity = 1, sanPham= sanpham});
            }

            // Lưu cart vào Session
            _notifyService.Success("Đã thêm sản phẩm vầo giỏ hàng.");

            SaveCartSession(cart);
            // Chuyển đến trang hiện thị Cart
            return RedirectToAction(nameof(Cart));
        }

        [Route("/cart", Name = "cart")]
        public IActionResult Cart()
        {
            ViewData["DanhMuc"] = new SelectList(_context.LoaiSanPham, "Id", "Name");

            return View(GetCartItems());
        }

        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromForm] int productid, [FromForm] int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.sanPham.Id == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity = quantity;
            }
            SaveCartSession(cart);
            _notifyService.Success("Cập nhật thành công.");

            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }
        [Route("/removecart/{productid:int}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] int productid)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.sanPham.Id == productid);
            if (cartitem != null)
            {
                cart.Remove(cartitem);
            }
            _notifyService.Success("Xoá thành công.");

            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }
    }
}