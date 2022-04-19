using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using App.Models;

namespace App.Admin.Role
{
  [Authorize(Roles = "Admin")]
  public class IndexModel : RolePageModel
  {
    public class RoleModel : IdentityRole
    {
      public string[] Claims { get; set; }
    }

    public List<RoleModel> roles { get; set; }
    public IndexModel(RoleManager<IdentityRole> roleManage, AppDbContext myBlogContext) : base(roleManage, myBlogContext)
    {
    }
    public async Task OnGet()
    {
      var r = await _roleManage.Roles.OrderBy(r => r.Name).ToListAsync();
      roles = new List<RoleModel>();
      foreach (var _r in r)
      {
        var claims = await _roleManage.GetClaimsAsync(_r);
        var claimsString = claims.Select(c =>c.Type + "="+ c.Value);

        var rm = new RoleModel()
        {
          Name = _r.Name,
          Id = _r.Id,
          Claims = claimsString.ToArray()
        };
        roles.Add(rm);
      }
    }

    public void OnPost() => RedirectToPage();
  }
}
