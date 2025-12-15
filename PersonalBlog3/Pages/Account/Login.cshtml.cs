using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonalBlog3.Data;
using PersonalBlog3.Models;
using System.Threading.Tasks;

namespace PersonalBlog3.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly BlogDbContext _context;

        public LoginModel(BlogDbContext context)
        {
            _context = context;
        }

        // 绑定登录表单数据
        [BindProperty]
        public User LoginUser { get; set; } = new();

        // 登录失败提示
        public string ErrorMessage { get; set; } = "";

        // GET请求：加载登录页面
        public void OnGet()
        {
            // 若已登录，直接跳首页
            if (HttpContext.Session.GetString("Username") != null)
            {
                Response.Redirect("/");
            }
        }

        // POST请求：处理登录逻辑
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "请输入完整的账号和密码！";
                return Page();
            }

            // 查找用户（匹配用户名+密码）
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == LoginUser.Username && u.Password == LoginUser.Password);

            if (user == null)
            {
                ErrorMessage = "用户名或密码错误！";
                return Page();
            }

            // 登录成功：设置Session
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString()); // 管理员标识

            // 跳转到首页
            return RedirectToPage("/Index");
        }
    }
}