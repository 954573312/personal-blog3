using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalBlog3.Data;
using PersonalBlog3.Models;
using System.Threading.Tasks;

namespace PersonalBlog3.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly BlogDbContext _context;

        public CreateModel(BlogDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; } = new();

        public string Message { get; set; } = "";

        // GET：加载添加分类页面
        public void OnGet()
        {
            // 未登录则跳登录页
            if (HttpContext.Session.GetString("Username") == null)
            {
                Response.Redirect("/Account/Login");
            }
        }

        // POST：处理添加分类
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "请输入分类名称！";
                return Page();
            }

            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();

            // 添加成功跳首页
            return RedirectToPage("/Index");
        }
    }
}