using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using App.Models;

namespace App.Pages;

public class IndexModel : PageModel
{
  private readonly ILogger<IndexModel> _logger;
  private readonly AppDbContext _myBlogContext;

  public IndexModel(ILogger<IndexModel> logger, AppDbContext myBlogContext)
  {
    _myBlogContext = myBlogContext;
    _logger = logger;
  }

  public void OnGet()
  {
    var posts = (from a in _myBlogContext.articles
                 orderby a.Created descending
                 select a).ToList();

    ViewData["posts"] = posts;
  }
}
