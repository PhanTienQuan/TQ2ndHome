using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TQuanHome.Areas.Identity.Data;

namespace TQuanHome.Controllers
{
    [Authorize]
    public class MyPostController : Controller
    {
        private readonly TQuanHomeContext _context;
        private readonly UserManager<TQuanHomeUser> _userManager;
        private readonly ILogger<MyPostController> _logger;
        public MyPostController(TQuanHomeContext context, UserManager<TQuanHomeUser> userManager, ILogger<MyPostController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
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

            if (posts == null || posts.Count == 0)
            {
                _logger.LogWarning("No posts found for user {UserName}", user.UserName);
                // Trả về view rỗng hoặc thông báo cho người dùng
                return View(new List<PostInfo>());
            }
            _logger.LogInformation("Found {Count} posts for user {UserName}", posts.Count, user.UserName);
            // Trả về view với dữ liệu posts
            return View(posts);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteMyPost(int postId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                // Kiểm tra xem tin đăng có tồn tại trong danh sách đã lưu của người dùng không
                var myPost = await _context.PostInfos.FirstOrDefaultAsync(s => s.PostId == postId);
                if (myPost == null)
                {
                    _logger.LogWarning("Saved post with ID {PostId} not found for user {UserName}", postId, user.UserName);
                    return NotFound();
                }

                _context.PostInfos.Remove(myPost);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Post with ID  removed");

                return Ok(new { deleted = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting saved post");
                return StatusCode(500, "Internal server error");
            }
        }

    }
    //  return View("~/Areas/Identity/Pages/Account/Manage/MyPost.cshtml", userPosts);
}
