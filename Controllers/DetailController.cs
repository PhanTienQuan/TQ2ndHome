using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TQuanHome.Areas.Identity.Data;
using TQuanHome.ViewModels;

namespace TQuanHome.Controllers
{
    public class DetailController : Controller
        
    {
        private readonly TQuanHomeContext _context;
        private readonly UserManager<TQuanHomeUser> _userManager; // Thêm UserManager để lấy thông tin người dùng
        private readonly ILogger<DetailController> _logger;

        public DetailController(TQuanHomeContext context, UserManager<TQuanHomeUser> userManager, ILogger<DetailController> logger)
        {
            _context = context;
            _logger = logger; // Assign logger
            _userManager = userManager;
        }

        public IActionResult Index(int id)
        {
            var postItem = _context.PostInfos.FirstOrDefault(p => p.PostId == id);
            var user = _userManager.GetUserAsync(User).Result;
            if (postItem == null)
            {
                _logger.LogWarning("Post with ID {Id} not found", id);
                return NotFound();
            }

            if (user != null)
            {
                var savedPost = _context.SavedPosts.FirstOrDefault(s => s.PostId == id && s.UserName == user.UserName);
                if (savedPost != null)
                {
                    ViewData["IsSavedClass"] = "saved";
                }
                else
                {
                    ViewData["IsSavedClass"] = "not-saved";
                }
            }
            else
            {
                ViewData["IsSavedClass"] = "not-saved";
            }

            return View(postItem);
        }

        //save pots
        [HttpPost]
        public async Task<IActionResult> SavePost([FromBody] SavePostVM model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                _logger.LogInformation("Received SavePost request with PostId: {PostId}, CurrentUser: {UserName}", model.PostId, user.UserName);

                var existingSavedPost = _context.SavedPosts.FirstOrDefault(s => s.PostId == model.PostId && s.UserName == user.UserName);
                if (existingSavedPost != null)
                {
                    _context.SavedPosts.Remove(existingSavedPost);
                    await _context.SaveChangesAsync();
                    return Ok(new { saved = false });
                }
                else
                {
                    var newSavedPost = new SavedPost
                    {
                        PostId = model.PostId,
                        UserName = user.UserName,
                        SaveDate = DateTime.Now
                    };
                    _context.SavedPosts.Add(newSavedPost);
                    await _context.SaveChangesAsync();
                    return Ok(new { saved = true });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving post");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
