using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using App.Models;

namespace App.Admin.Role
{
  [Authorize(Roles ="Admin")]
  public class DeleteModel : RolePageModel
  {
    public DeleteModel(RoleManager<IdentityRole> roleManage, AppDbContext myBlogContext) : base(roleManage, myBlogContext)
    {
    }
    public IdentityRole role { get; set; }

    public async Task<IActionResult> OnGet(string roleid)
    {
      if (roleid == null) return NotFound("Không tìm thấy role");
      role = await _roleManage.FindByIdAsync(roleid);
      if (role == null)
      {
        return NotFound("Không tìm thấy role");
      }
      else
      {
        return Page();
      }

    }
    public async Task<IActionResult> OnPostAsync(string roleid)
    {
      if (roleid == null) return NotFound("Không tìm thấy role");
      role = await _roleManage.FindByIdAsync(roleid);
      if (role == null) return NotFound("Không tìm thấy role");

      var result = await _roleManage.DeleteAsync(role);

      if (result.Succeeded)
      {
        StatusMessage = $"Bạn vừa xoá role: {role.Name}";
        return RedirectToPage("./Index");
      }
      else
      {
        result.Errors.ToList().ForEach(error =>
        {
          ModelState.AddModelError(string.Empty, error.Description);
        });
      }
      return Page();
    }
  }
}
