using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using App.Models;

namespace App.Admin.User
{
  [Authorize]
  public class IndexModel : PageModel
  {
    private readonly UserManager<AppUser> _userManager;
    public IndexModel(UserManager<AppUser> userManager)
    {
      _userManager = userManager;
    }
    public class UserAndRole : AppUser
    {
      public string RoleNames { get; set; }
    }
    public List<UserAndRole> users { get; set; }
    [TempData]
    public string StatusMessage { get; set; }

    public const int ITEMS_PER_PAGE = 10;

    [BindProperty(SupportsGet = true, Name = "p")]
    public int currentPage { get; set; }
    public int countPages { get; set; }
    public int totalUser { get; set; }
    public async Task OnGet()
    {
      // users = await _userManager.Users.OrderBy(u => u.UserName).ToListAsync();
      var query = _userManager.Users.OrderBy(u => u.UserName);

      totalUser = await query.CountAsync();
      countPages = (int)Math.Ceiling((double)totalUser / ITEMS_PER_PAGE);

      if (currentPage < 1)
        currentPage = 1;
      if (currentPage > countPages && countPages > 0)
        currentPage = countPages;

      var item = query.Skip((currentPage - 1) * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE)
      .Select(u => new UserAndRole
      {
        Id = u.Id,
        UserName = u.UserName
      });
      users = await item.ToListAsync();

      foreach (var user in users)
      {
        var roles = await _userManager.GetRolesAsync(user);
        user.RoleNames = string.Join(",", roles);
      }
    }

    public void OnPost() => RedirectToPage();
  }
}
