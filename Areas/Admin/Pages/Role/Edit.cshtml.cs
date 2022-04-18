using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using razorweb.models;

namespace App.Admin.Role
{
  [Authorize(Roles ="Admin")]
  public class EditModel : RolePageModel
  {
    public EditModel(RoleManager<IdentityRole> roleManage, MyBlogContext myBlogContext) : base(roleManage, myBlogContext)
    {
    }
    public class InputModel
    {
      [Display(Name = "Tên của role")]
      [Required(ErrorMessage = "Phải nhập {0}")]
      [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} đến {1} kí tự")]
      public string Name { get; set; }
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public IdentityRole role { get; set; }

    public async Task<IActionResult> OnGet(string roleid)
    {
      if (roleid == null) return NotFound("Không tìm thấy role");
      role = await _roleManage.FindByIdAsync(roleid);
      if (role != null)
      {
        Input = new InputModel
        {
          Name = role.Name
        };
        return Page();
      }
      return NotFound("Không tìm thấy role");
    }
    public async Task<IActionResult> OnPostAsync(string roleid)
    {
      if (roleid == null) return NotFound("Không tìm thấy role");
      role = await _roleManage.FindByIdAsync(roleid);
      if (role == null) return NotFound("Không tìm thấy role");
      if (!ModelState.IsValid)
      {
        return Page();
      }

      role.Name = Input.Name;
      var result = await _roleManage.UpdateAsync(role);

      if (result.Succeeded)
      {
        StatusMessage = $"Bạn vừa đổi tên: {Input.Name}";
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