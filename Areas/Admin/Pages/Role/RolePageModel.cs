using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razorweb.models;

namespace App.Admin.Role
{
  public class RolePageModel : PageModel
  {
    protected readonly RoleManager<IdentityRole> _roleManage;
    protected readonly MyBlogContext _context;

    [TempData]
    public string StatusMessage { get; set; }
    public RolePageModel(RoleManager<IdentityRole> roleManage, MyBlogContext myBlogContext)
    {
      _context = myBlogContext;
      _roleManage = roleManage;
    }
  }
}