using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonalBlog3.Data;
using PersonalBlog3.Models;
using System.Threading.Tasks;

namespace PersonalBlog3.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly BlogDbContext _context;

        public RegisterModel(BlogDbContext context)
        {
            _context = context;
        }

        // 绑定注册表单数据
        [BindProperty]
        public User RegisterUser { get; set; } = new();

        // 注册提示信息
        public string Message { get; set; } = "";
        public string MessageType { get; set; } = "danger"; // success/danger

        // GET请求：加载注册页面
        public void OnGet()
        {
            // 若已登录，直接跳首页
            if (HttpContext.Session.GetString("Username") != null)
            {
                Response.Redirect("/");
            }
        }

        // POST请求：处理注册逻辑
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "请输入完整的注册信息！";
                return Page();
            }

            // 检查用户名是否已存在
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == RegisterUser.Username);

            if (existingUser != null)
            {
                Message = "用户名已存在！请更换用户名";
                return Page();
            }

            // 新增用户（默认普通用户，IsAdmin=false）
            var newUser = new User
            {
                Username = RegisterUser.Username,
                Password = RegisterUser.Password,
                IsAdmin = false // 如需创建管理员，可手动设为true
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // 注册成功
            Message = "注册成功！即将跳转到登录页...";
            MessageType = "success";

            // 3秒后跳登录页
            Response.Headers.Append("Refresh", "3;URL=/Account/Login");

            return Page();
        }
    }
}