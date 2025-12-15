using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonalBlog3.Data;
using PersonalBlog3.Models;
using System.Threading.Tasks;

namespace PersonalBlog3.Pages.Posts
{
    public class CreateModel : PageModel
    {
        private readonly BlogDbContext _context;

        // 构造函数注入数据库上下文
        public CreateModel(BlogDbContext context)
        {
            _context = context;
        }

        // 绑定发布文章的表单数据
        [BindProperty]
        public Post Post { get; set; } = new();

        // 分类列表（下拉框用）
        public List<Category> Categories { get; set; } = new();

        // 登录提示信息
        public string LoginMessage { get; set; } = "";

        // GET请求：加载发布页面（优先校验登录）
        public async Task OnGetAsync()
        {
            // 核心：判断是否登录（Session中是否有用户名）
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                // 未登录：设置提示信息，不加载分类
                LoginMessage = "请先登录后再发布文章！";
                return;
            }

            // 已登录：加载分类列表
            Categories = await _context.Categories.ToListAsync();
        }

        // POST请求：提交发布（优先校验登录）
        public async Task<IActionResult> OnPostAsync()
        {
            // 核心：未登录直接跳登录页
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToPage("/Account/Login");
            }

            // 表单验证失败
            if (!ModelState.IsValid)
            {
                Categories = await _context.Categories.ToListAsync();
                return Page();
            }

            // 填充文章信息
            Post.Author = username;
            Post.CreatedTime = DateTime.Now;

            // 保存到数据库
            _context.Posts.Add(Post);
            await _context.SaveChangesAsync();

            // 发布成功跳首页
            return RedirectToPage("/Index");
        }
    }
}