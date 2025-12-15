using System.ComponentModel.DataAnnotations;

namespace PersonalBlog3.Models
{
    public class User
    {
        // 关键修正：移除int类型Id的MaxLength（数值类型无需长度限制，SQLite不兼容）
        public int Id { get; set; }

        [Required]
        [Display(Name = "用户名")]
        // 补充：添加MaxLength适配SQLite，避免varchar(max)语法
        [MaxLength(int.MaxValue)]
        // 补充：给字符串字段设置默认空字符串，避免空引用异常
        public string Username { get; set; } = string.Empty;

        [Required]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        // 补充：添加MaxLength适配SQLite
        [MaxLength(int.MaxValue)]
        // 补充：默认空字符串，避免空引用
        public string Password { get; set; } = string.Empty;

        [Display(Name = "创建时间")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        // 新增：管理员标识（默认false）
        public bool IsAdmin { get; set; } = false;
    }
}