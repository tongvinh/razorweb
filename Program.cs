
using App.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using razorweb.models;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MyBlogContext>(options =>
{
  string connectionString = configuration.GetConnectionString("MyBlogContext");
  options.UseSqlServer(connectionString);
});

//Đăng ký Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
  .AddEntityFrameworkStores<MyBlogContext>()
  .AddDefaultTokenProviders();

// builder.Services.AddDefaultIdentity<AppUser>()
//   .AddEntityFrameworkStores<MyBlogContext>()
//   .AddDefaultTokenProviders();

// Truy cập IdentityOptions
builder.Services.Configure<IdentityOptions>(options =>
{
  // Thiết lập về Password
  options.Password.RequireDigit = false; // Không bắt phải có số
  options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
  options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
  options.Password.RequireUppercase = false; // Không bắt buộc chữ in
  options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
  options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

  // Cấu hình Lockout - khóa user
  options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
  options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 5 lầ thì khóa
  options.Lockout.AllowedForNewUsers = true;

  // Cấu hình về User.
  options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
      "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
  options.User.RequireUniqueEmail = true; // Email là duy nhất

  // Cấu hình đăng nhập.
  options.SignIn.RequireConfirmedEmail = true; // Cấu hình xác thực địa chỉ email (email phải tồn tại)
  options.SignIn.RequireConfirmedPhoneNumber = false; // Xác thực số điện thoại
  options.SignIn.RequireConfirmedAccount = true;

});

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
  //Trên 30 giây truy cập lại sẽ nạp lại thông tin User (Role)
  //SercurityStamp trong bảng User đổi -> nạp lại thông tin Security
  options.ValidationInterval = TimeSpan.FromSeconds(30);
});

builder.Services.ConfigureApplicationCookie(options =>
{
  options.LoginPath = "/login/";
  options.LogoutPath = "/logout/";
  options.AccessDeniedPath = "/khongduoctruycap.html";
});

//Đăng nhập bên thứ 3
builder.Services.AddAuthentication()
  .AddGoogle(googleOptions =>
  {
    // Đọc thông tin Authentication:Google từ appsettings.json
    IConfigurationSection googleAuthNsection = configuration.GetSection("Authentication:Google");

    // Thiết lập ClientID và ClientSecret để truy cập API google
    googleOptions.ClientId = googleAuthNsection["ClientId"];
    googleOptions.ClientSecret = googleAuthNsection["ClientSecret"];
    // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
    googleOptions.CallbackPath = "/dang-nhap-tu-google";
  });

builder.Services.AddOptions();                                //Kích hoat Options
var mailsettings = configuration.GetSection("MailSettings");  // Đọc config
builder.Services.Configure<MailSettings>(mailsettings);       //Đăng ký đê inject
// Đăng ký SendMailService với kiểu Transient, mỗi lần gọi dịch
// vụ ISendMailService một đới tượng SendMailService tạo ra (đã inject config)
builder.Services.AddSingleton<IEmailSender, SendMailService>();
builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

/*
CREATE, READ, UPDATE, DELETE (CRUD)

dotnet aspnet-codegenerator razorpage -m razorweb.models.Article -dc razorweb.models.MyBlogContext -outDir Pages/Blog -udl --referenceScriptLibraries

Identity:
  - Authentication: Xác định danh tính -> Login, Logout ...
  - Authorization: Xác thực quyền truy cập
    Role-based authorization - xác thự quyền theo vai trò
      -Role (vai trò) : (Admin, Editor, Manager, Member ...)
    /Areas/Admin/Pages/Role
    Index
    Create
    Edit
    Delete

    dotnet new page -n Index -o /Areas/Admin/Pages/Role -na App.Admin.Role
    dotnet new page -n Create -o /Areas/Admin/Pages/Role -na App.Admin.Role

    [Authorize] - Controller, Action, PageModel --> user login (dang nhap)

  - Quản lý user: Sign Up, User, Role ...
  /Identity/Account/Login
  /Identity/Account/Manage

  dotnet aspnet-codegenerator identity -dc razorweb.models.MyBlogContext

CallbackPath
  https://localhost:5000/dang-nhap-tu-google
*/