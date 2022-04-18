using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using razorweb.models;

namespace App.Admin.Role
{
  [Authorize(Roles = "Admin")]
  public class EditRoleClaimModel : RolePageModel
  {
    public EditRoleClaimModel(RoleManager<IdentityRole> roleManage, MyBlogContext myBlogContext) : base(roleManage, myBlogContext)
    {
    }
    public class InputModel
    {
      [Display(Name = "Kiểu (tên) claim")]
      [Required(ErrorMessage = "Phải nhập {0}")]
      [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} đến {1} kí tự")]
      public string ClaimType { get; set; }

      [Display(Name = "Giá trị")]
      [Required(ErrorMessage = "Phải nhập {0}")]
      [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} đến {1} kí tự")]
      public string ClaimValue { get; set; }
    }

    [BindProperty]
    public InputModel Input { get; set; }
    public IdentityRole role { get; set; }
    IdentityRoleClaim<string> claim { get; set; }

    public async Task<IActionResult> OnGet(int? claimid)
    {
      if (claimid == null) return NotFound("Không tìm thấy role");
      claim = _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
      if (claim == null) return NotFound("KHông tìm thấy role");

      role = await _roleManage.FindByIdAsync(claim.RoleId);
      if (role == null) return NotFound("Không tìm thấy role");

      Input = new InputModel
      {
        ClaimType = claim.ClaimType,
        ClaimValue = claim.ClaimValue
      };
      return Page();
    }
    public async Task<IActionResult> OnPostAsync(int? claimid)
    {

      if (claimid == null) return NotFound("Không tìm thấy role");
      claim = _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
      if (claim == null) return NotFound("KHông tìm thấy role");

      role = await _roleManage.FindByIdAsync(claim.RoleId);
      if (role == null) return NotFound("Không tìm thấy role");

      if (!ModelState.IsValid)
      {
        return Page();
      }
      if (_context.RoleClaims.Any(c => c.RoleId == role.Id && c.ClaimType == Input.ClaimType
      && c.ClaimValue == Input.ClaimValue && c.Id != claim.Id))
      {
        ModelState.AddModelError(string.Empty, "Claim này đã có trong role");
        return Page();
      }

      claim.ClaimType = Input.ClaimType;
      claim.ClaimValue = Input.ClaimValue;

      await _context.SaveChangesAsync();
      StatusMessage = "Vừa cập nhật claim";

      return RedirectToPage("./Edit", new { roleid = role.Id });
    }
   public async Task<IActionResult> OnPostDeleteAsync(int? claimid)
    {

      if (claimid == null) return NotFound("Không tìm thấy role");
      claim = _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
      if (claim == null) return NotFound("KHông tìm thấy role");

      role = await _roleManage.FindByIdAsync(claim.RoleId);
      if (role == null) return NotFound("Không tìm thấy role");

    
      await _roleManage.RemoveClaimAsync(role, new Claim(claim.ClaimType, claim.ClaimValue));
      StatusMessage = "Vừa xoá claim";

      return RedirectToPage("./Edit", new { roleid = role.Id });
    }
  }
}
