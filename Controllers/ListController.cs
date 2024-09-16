using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TQuanHome.Areas.Identity.Data;
using TQuanHome.ViewModels;
using X.PagedList;
using System.Linq;
using System.Threading.Tasks;


namespace TQuanHome.Controllers
{
    public class ListController : Controller
    {
        private readonly TQuanHomeContext _context;
        public ListController(TQuanHomeContext context) 
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 4;
            int pageNumber = page ?? 1;

            /*var posts = await _context.PostInfos
                .OrderByDescending(p => p.PostDate)
                .ToPagedListAsync(pageNumber, pageSize);*/

            var posts = await _context.PostInfos
            .Include(p => p.Province)
            .Include(p => p.District)
            .Include(p => p.Ward)
            .OrderByDescending(p => p.PostDate)
            .ToPagedListAsync(pageNumber, pageSize);

            return View(posts);
        
            var pagedPosts = await posts.OrderByDescending(p => p.PostDate).ToPagedListAsync(pageNumber, pageSize);

            return View(pagedPosts);
        }

        public  IActionResult Detail(int id)
        {
            var postItem = _context.PostInfos
                .Include(p => p.Province)
                .Include(p => p.District)
                .Include(p => p.Ward)
                .FirstOrDefault(p => p.PostId == id);

            if (postItem == null)
            {
                return NotFound();
            }

            return View(postItem);
        }
    }
}
