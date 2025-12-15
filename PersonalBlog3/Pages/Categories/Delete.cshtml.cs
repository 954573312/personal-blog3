using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonalBlog3.Data;
using PersonalBlog3.Models;
using System.Threading.Tasks;

namespace PersonalBlog3.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly BlogDbContext _context;

        public DeleteModel(BlogDbContext context)
        {
            _context = context;
        }

        // 分类数据（前端展示用）
        public Category Category { get; set; } = new();

        // 提示信息
        public string Message { get; set; } = "";
        public string MessageType { get; set; } = "danger";

        // GET请求：加载删除确认页（校验管理员权限）
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // 1. 未登录 → 跳登录页
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToPage("/Account/Login");
            }

            // 2. 非管理员 → 提示无权限
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";
            if (!isAdmin)
            {
                Message = "只有管理员才能删除分类！";
                return Page();
            }

            // 3. 查找分类
            Category = await _context.Categories.FindAsync(id);
            if (Category == null)
            {
                Message = "分类不存在！";
                return Page();
            }

            return Page();
        }

        // POST请求：执行删除（校验管理员权限）
        public async Task<IActionResult> OnPostAsync(int id)
        {
            // 1. 校验管理员权限
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";
            if (!isAdmin)
            {
                Message = "只有管理员才能删除分类！";
                return Page();
            }

            // 2. 查找并删除分类
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                Message = "分类删除成功！";
                MessageType = "success";
                // 3秒后跳首页
                Response.Headers.Append("Refresh", "3;URL=/");
            }
            else
            {
                Message = "分类不存在！";
            }
            // 新增：校验分类下是否有文章
            var hasPosts = await _context.Posts.AnyAsync(p => p.CategoryId == id);
            if (hasPosts)
            {
                Message = "该分类下有文章，无法删除！";
                return Page();
            }
            return Page();
        }
    }
}