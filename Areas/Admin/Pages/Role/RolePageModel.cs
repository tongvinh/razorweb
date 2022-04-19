using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using App.Models;

namespace App.Admin.Role
{
  public class RolePageModel : PageModel
  {
    protected readonly RoleManager<IdentityRole> _roleManage;
    protected readonly AppDbContext _context;

    [TempData]
    public string StatusMessage { get; set; }
    public RolePageModel(RoleManager<IdentityRole> roleManage, AppDbContext myBlogContext)
    {
      _context = myBlogContext;
      _roleManage = roleManage;
    }
  }
}