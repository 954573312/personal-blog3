using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonalBlog3.Data;
using PersonalBlog3.Models;
using System.Threading.Tasks;

namespace PersonalBlog3.Pages
{
    // 首页Razor Page的后台逻辑类
    public class IndexModel : PageModel
    {
        // 注入数据库上下文（和HomeController一致）
        private readonly BlogDbContext _context;

        public IndexModel(BlogDbContext context)
        {
            _context = context;
        }


        // --- 页面需要的变量（对应HomeController中的数据，前端用@Model.XXX访问） ---
        // 文章列表
        public List<Post> Posts { get; set; } = new();
        // 分类列表
        public List<Category> Categories { get; set; } = new();
        // 当前分类名称
        public string CurrentCategoryName { get; set; } = "";
        // 是否登录
        public bool IsLoggedIn { get; set; }
        // 登录用户名
        public string Username { get; set; } = "";
        // 是否是管理员
        public bool IsAdmin { get; set; }


        // --- 处理首页GET请求（对应HomeController的Index方法） ---
        public async Task OnGetAsync(int? categoryId)
        {
            // 1. 筛选文章（复用HomeController的逻辑）
            IQueryable<Post> postsQuery = _context.Posts.Include(p => p.Category);
            if (categoryId.HasValue)
            {
                postsQuery = postsQuery.Where(p => p.CategoryId == categoryId.Value);
            }
            Posts = await postsQuery.ToListAsync();


            // 2. 获取分类列表 + 当前分类名称（复用HomeController的逻辑）
            Categories = await _context.Categories.ToListAsync();
            CurrentCategoryName = categoryId.HasValue
                ? (await _context.Categories.FindAsync(categoryId.Value))?.Name ?? ""
                : "最新文章";


            // 3. 判断登录状态 + 管理员身份（复用HomeController的Session逻辑）
            IsLoggedIn = HttpContext.Session.GetString("Username") != null;
            Username = HttpContext.Session.GetString("Username") ?? "";
            IsAdmin = HttpContext.Session.GetString("IsAdmin") == "True";
        }
    }
}