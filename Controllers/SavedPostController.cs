using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TQuanHome.Areas.Identity.Data;

namespace TQuanHome.Controllers
{
    [Authorize]
    public class SavedPostController : Controller
    {
        private readonly TQuanHomeContext _context;
        private readonly UserManager<TQuanHomeUser> _userManager;
        private readonly ILogger<SavedPostController> _logger;
        public SavedPostController(TQuanHomeContext context, UserManager<TQuanHomeUser> userManager, ILogger<SavedPostController> logger)
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

            // Lấy danh sách các tin đã lưu của người dùng
            var savedPosts = await _context.SavedPosts
                .Where(sp => sp.UserName == user.UserName)
                .ToListAsync();

            if (savedPosts == null || savedPosts.Count == 0)
            {
                _logger.LogWarning("No saved posts found for user {UserName}", user.UserName);
                return View(new List<PostInfo>()); // Trả về view rỗng hoặc thông báo cho người dùng
            }

            // Lấy danh sách các PostId của các tin đăng đã lưu
            var postIds = savedPosts.Select(sp => sp.PostId).ToList();

            // Lấy thông tin chi tiết của các tin đăng đã lưu
            var posts = await _context.PostInfos
                .Where(p => postIds.Contains(p.PostId))
                .ToListAsync();

            _logger.LogInformation("Found {Count} saved posts for user {UserName}", posts.Count, user.UserName);

            // Trả về view với dữ liệu posts
            return View(posts);
        }

        // POST: SavedPost/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteSavedPost(int postId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                // Kiểm tra xem tin đăng có tồn tại trong danh sách đã lưu của người dùng không
                var savedPost = await _context.SavedPosts.FirstOrDefaultAsync(s => s.PostId == postId && s.UserName == user.UserName);
                if (savedPost == null)
                {
                    _logger.LogWarning("Saved post with ID {PostId} not found for user {UserName}", postId, user.UserName);
                    return NotFound();
                }

                _context.SavedPosts.Remove(savedPost);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Post with ID {PostId} removed from saved posts for user {UserName}", postId, user.UserName);

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
