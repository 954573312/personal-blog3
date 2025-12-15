using System.ComponentModel.DataAnnotations;

namespace PersonalBlog3.Models
{
    public class Post
    {
        // 关键修正：移除int类型Id的MaxLength（数值类型无需长度限制，SQLite不兼容）
        public int Id { get; set; }

        [Required(ErrorMessage = "文章标题不能为空")]
        // 补充：给字符串字段添加MaxLength，适配SQLite（避免生成varchar(max)）
        [MaxLength(int.MaxValue)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "文章内容不能为空")]
        // 补充：给内容字段添加MaxLength，适配SQLite
        [MaxLength(int.MaxValue)]
        public string Content { get; set; } = string.Empty;

        // 默认作者，后续可改为登录用户
        [MaxLength(int.MaxValue)] // 补充：字符串字段统一添加MaxLength
        public string Author { get; set; } = "admin@example.com";

        // 发布时间，默认当前时间（DateTime类型无需额外配置）
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        // 关联分类（外键，int类型无需配置）
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}