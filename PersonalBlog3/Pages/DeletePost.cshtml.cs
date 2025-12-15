using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonalBlog3.Data;
using PersonalBlog3.Models;
using System.Threading.Tasks;

namespace PersonalBlog3.Pages
{
    public class DeletePostModel : PageModel
    {
        private readonly BlogDbContext _context;

        public DeletePostModel(BlogDbContext context)
        {
            _context = context;
        }

        // 页面提示信息（前端用@Model.Message访问）
        public string Message { get; set; } = "";
        public string MessageType { get; set; } = "danger"; // success/warning/danger

        public async Task OnGetAsync(int id)
        {
            // 1. 校验管理员权限
            bool isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";
            if (!isAdmin)
            {
                Message = "权限不足！只有管理员才能执行删除文章操作";
                return;
            }

            // 2. 查找文章
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                Message = "文章不存在！该文章可能已被删除或不存在";
                MessageType = "warning";
                return;
            }

            // 3. 执行删除
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            // 4. 删除成功
            Message = "文章删除成功！";
            MessageType = "success";
        }
    }
}