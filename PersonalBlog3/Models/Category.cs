using System.ComponentModel.DataAnnotations;

namespace PersonalBlog3.Models
{
    public class Category
    {
        // 移除Id字段的MaxLength（int类型无需长度限制）
        public int Id { get; set; }

        [Required(ErrorMessage = "分类名称不能为空")]
        // 保留Name字段的MaxLength（适配SQLite）
        [MaxLength(int.MaxValue)]
        public string Name { get; set; } = string.Empty;
    }
}