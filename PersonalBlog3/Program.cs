using Microsoft.EntityFrameworkCore;
using PersonalBlog3.Data;
using System; // 补充InvalidOperationException所需的命名空间

var builder = WebApplication.CreateBuilder(args);

// 1. 保留Razor Pages支持
builder.Services.AddRazorPages();

// 2. 保留控制器和视图支持
builder.Services.AddControllersWithViews();

// 3. 核心修改：注册BlogDbContext为SQLite（删除原SQL Server配置）
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("BlogDbContext")
        ?? throw new InvalidOperationException("Connection string 'BlogDbContext' not found.")
    ));

// 4. 保留会话支持（登录状态）
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 会话超时时间
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// 配置HTTP请求管道
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.MapRazorPages();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 启用会话（必须在UseRouting之后，UseAuthorization之前）
app.UseSession();

app.UseAuthorization();

// 配置默认路由
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();