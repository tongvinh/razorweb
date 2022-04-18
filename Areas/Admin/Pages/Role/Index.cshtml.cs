using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using razorweb.models;

namespace App.Admin.Role
{
  [Authorize(Roles ="Admin")]
  public class IndexModel : RolePageModel
  {
    public List<IdentityRole> roles { get; set; }
    public IndexModel(RoleManager<IdentityRole> roleManage, MyBlogContext myBlogContext) : base(roleManage, myBlogContext)
    {
    }
    public async Task OnGet()
    {
      roles = await _roleManage.Roles.OrderBy(r => r.Name).ToListAsync();
    }

    public void OnPost() => RedirectToPage();
  }
}
