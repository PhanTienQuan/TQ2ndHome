using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TQuanHome.Areas.Identity.Data;
using TQuanHome.ViewModels;

namespace TQuanHome.Controllers
{
    public class AccountController : Controller
    {

        private readonly TQuanHomeContext _context;
        private readonly UserManager<TQuanHomeUser> _userManager; // Thêm UserManager để lấy thông tin người dùng
        private readonly ILogger<AccountController> _logger;
        public AccountController(TQuanHomeContext context, UserManager<TQuanHomeUser> userManager, ILogger<AccountController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger; // Assign logger
        }

        /// ////////////////////////////////////////////////////
        [HttpGet]
        public async Task<IActionResult> Post()
        {
            _logger.LogInformation("Get: Post");
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                // Xử lý khi không tìm thấy người dùng
                return RedirectToAction("Login", "Account");
            }

            var provinces = await _context.Provinces.ToListAsync(); // Lấy danh sách tỉnh từ database

            var postCompositeVM = new PostCompositeVM
            {
                Provinces = provinces
            };

            return View(postCompositeVM);
        }

       
        [HttpPost]
        public async Task<IActionResult> Post(PostCompositeVM model)
        {
            // Log thông tin request
            _logger.LogInformation("Received POST request for Post action");
            // Lấy thông tin người dùng hiện tại
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound("Không tìm thấy thông tin người dùng.");
            }

            if (ModelState.IsValid)
            {
                // Lấy thông tin người dùng hiện tại
                var userName = user.UserName;
                // Handle the file uploads and save paths
                var image1Path = await SaveFileAsync(model.PostVM.Image1);
                var image2Path = await SaveFileAsync(model.PostVM.Image2);
                var image3Path = await SaveFileAsync(model.PostVM.Image3);

                // Create a new PostInfo entity
                var postInfo = new PostInfo
                {
                    UserName = userName,
                    /*UserName = "quan@gmail.com",*/
                    PostTitle = model.PostVM.PostTitle,
                    ProvinceId = model.PostVM.ProvinceId,
                    DistrictId = model.PostVM.DistrictId,
                    WardId = model.PostVM.WardId,
                    AddressDetail = model.PostVM.Address,
                    Price = model.PostVM.Price,
                    Description = model.PostVM.Description,
                    PostDate = DateTime.Now,
                    Status = 0,
                    IsFull = 0,
                    // Assign the image paths
                    Img1 = image1Path,
                    Img2 = image2Path,
                    Img3 = image3Path
                };

                // Save to database
                _context.PostInfos.Add(postInfo);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Data saved successfully.");

                TempData["SuccessMessage"] = "Đăng tin thành công!";
                return RedirectToAction("Post");
            }
            else
            {
                _logger.LogWarning("Model ís Invalid");
                // Log ModelState errors
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogError(error.ErrorMessage);
                    }
                }
            }

            // If we got this far, something failed; redisplay form.
            var provinces = await _context.Provinces.ToListAsync(); // Lấy danh sách tỉnh từ database
            var postCompositeVM = new PostCompositeVM
            {
                Provinces = provinces,
                PostVM = model.PostVM // Giữ lại các giá trị đã nhập
            };
            return View(model);
        }


        /////////////////////////////////////////////////////////////////////////////////

        //get district base on province
        [HttpGet]
        public JsonResult GetDistricts(int provinceId)
        {
            var districts = _context.Districts.Where(d => d.ProvinceId == provinceId).ToList();
            return Json(districts);
        }
        //get ward
        [HttpGet]
        public JsonResult GetWards(int districtId)
        {
            var wards = _context.Wards.Where(w => w.DistrictId == districtId).ToList();
            return Json(wards);
        }

        private async Task<string> SaveFileAsync(IFormFile file)
        {

            if (file == null || file.Length == 0)
                return null;

            // Tạo tên file mới để tránh trùng lặp
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Tạo đường dẫn tới thư mục wwwroot/images/Posts
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Posts");

            // Tạo thư mục nếu nó không tồn tại
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var path = Path.Combine(folderPath, uniqueFileName);

            // Lưu file
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Trả về đường dẫn tương đối của file
            return $"/images/Posts/{uniqueFileName}";
        }

        //////////////////////MY POTS///////////////
        public async Task<IActionResult> MyPost()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy danh sách các tin đăng của người dùng
            var posts = await _context.PostInfos
                .Where(p => p.UserName == user.UserName)
                .ToListAsync();

            // Trả về view với dữ liệu posts
            return View(posts);
        }
    }
}
