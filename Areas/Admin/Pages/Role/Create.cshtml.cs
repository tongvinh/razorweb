using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using razorweb.models;

namespace App.Admin.Role
{
  [Authorize(Roles ="Admin")]
  public class CreateModel : RolePageModel
  {
    public CreateModel(RoleManager<IdentityRole> roleManage, MyBlogContext myBlogContext) : base(roleManage, myBlogContext)
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

    public void OnGet()
    {
    }
    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }
      var newRole = new IdentityRole(Input.Name);
      var result = await _roleManage.CreateAsync(newRole);
      if (result.Succeeded)
      {
        StatusMessage = $"Bạn vừa tạo role mới: {Input.Name}";
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
