using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonalBlog3.Data;
using PersonalBlog3.Models;
using System.Threading.Tasks;

namespace PersonalBlog3.Pages
{
    public class PostDetailModel : PageModel
    {
        private readonly BlogDbContext _context;

        public PostDetailModel(BlogDbContext context)
        {
            _context = context;
        }

        // 文章数据（前端用@Model.Post访问）
        public Post Post { get; set; } = new();

        // 处理GET请求（接收id参数）
        public async Task OnGetAsync(int id)
        {
            // 查找文章（复用原有逻辑）
            Post = await _context.Posts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}