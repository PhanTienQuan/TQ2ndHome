using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TQuanHome.Areas.Identity.Data;
using TQuanHome.Models;

namespace TQuanHome.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TQuanHomeContext _context; 
        private readonly UserManager<TQuanHomeUser> _userManager;

        public HomeController(ILogger<HomeController> logger, TQuanHomeContext context, UserManager<TQuanHomeUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        /*public async Task<IActionResult>  Index()
        {
            // Lấy các tin đăng đề xuất
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var currUser = user.UserName;
            var recommendedPosts = GetRecommendedPosts(currUser);
            return View(recommendedPosts);
        }*/

        public async Task<IActionResult> Index()
        {
            // Lấy thông tin người dùng hiện tại
            var user = await _userManager.GetUserAsync(HttpContext.User);

            // Kiểm tra nếu người dùng đăng nhập (user khác null)
            if (user != null)
            {
                var currUser = user.UserName;
                var recommendedPosts = GetRecommendedPosts(currUser);
                return View(recommendedPosts);
            }
            else
            {
                // Xử lý khi người dùng không đăng nhập, ví dụ chuyển hướng đến trang đăng nhập
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
        }


        // Hàm tính toán đề xuất các tin đăng
        private List<PostInfo> GetRecommendedPosts(string userName)
        {
            // Lấy dữ liệu số lượt lưu tin đăng của người dùng
            var userSavedPosts = _context.SavedPosts.Where(sp => sp.UserName == userName).ToList();

            // Lấy tất cả các tin đăng đã được lưu bởi các người dùng khác
            var otherSavedPosts = _context.SavedPosts.Where(sp => sp.UserName != userName).ToList();

            // Tạo dictionary để lưu trữ điểm số tương tự (cosine similarity) của mỗi tin đăng
            var postSimilarities = new Dictionary<int, double>();

            foreach (var otherPost in otherSavedPosts)
            {
                if (!postSimilarities.ContainsKey(otherPost.PostId))
                    postSimilarities[otherPost.PostId] = 0;

                // Tính số lượng người dùng đã lưu cả hai tin đăng
                int commonSaves = userSavedPosts.Count(sp => sp.PostId == otherPost.PostId);

                // Tính cosine similarity
                double cosineSimilarity = (double)commonSaves / Math.Sqrt(userSavedPosts.Count * otherSavedPosts.Count);
                postSimilarities[otherPost.PostId] += cosineSimilarity;
            }

            // Sắp xếp các tin đăng dựa trên độ tương tự giảm dần
            var recommendedPostIds = postSimilarities.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).ToList();

            // Lấy thông tin chi tiết của các tin đăng được đề xuất
            var recommendedPosts = _context.PostInfos
                                         .Where(pi => recommendedPostIds.Contains(pi.PostId))
                                         .ToList();

            return recommendedPosts;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
