using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PersonalBlog3.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // 清除Session
            HttpContext.Session.Clear();
            // 跳转到首页
            return RedirectToPage("/Index");
        }
    }
}