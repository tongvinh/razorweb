using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using App.Models;

namespace App.Pages_Blog
{
  public class EditModel : PageModel
  {
    private readonly App.Models.AppDbContext _context;
    private readonly IAuthorizationService _authorizationService;

    public EditModel(App.Models.AppDbContext context, IAuthorizationService authorizationService)
    {
      _authorizationService = authorizationService;
      _context = context;
    }

    [BindProperty]
    public Article Article { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
      if (id == null)
      {
        return Content("Không thấy bài viết");
      }

      Article = await _context.articles.FirstOrDefaultAsync(m => m.Id == id);

      if (Article == null)
      {
        return Content("Không thấy bài viêt");
      }
      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      _context.Attach(Article).State = EntityState.Modified;

      try
      {
        //Kiểm tra quyen cap nhat
        var canupdate = await _authorizationService.AuthorizeAsync(this.User, Article, "CanUpdateArticle");
        if (canupdate.Succeeded)
        {
          await _context.SaveChangesAsync();
        }
        else
        {
          return Content("Không được quyền cập nhật");
        }
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!ArticleExists(Article.Id))
        {
          return Content("Không tìm thấy bài viết");
        }
        else
        {
          throw;
        }
      }

      return RedirectToPage("./Index");
    }

    private bool ArticleExists(int id)
    {
      return _context.articles.Any(e => e.Id == id);
    }
  }
}
