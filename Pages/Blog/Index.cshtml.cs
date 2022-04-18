using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorweb.models;

namespace razorweb.Pages_Blog
{
  [Authorize]
  public class IndexModel : PageModel
  {
    private readonly razorweb.models.MyBlogContext _context;

    public IndexModel(razorweb.models.MyBlogContext context)
    {
      _context = context;
    }

    public IList<Article> Article { get; set; }
    public const int ITEMS_PER_PAGE = 15;

    [BindProperty(SupportsGet = true, Name = "p")]
    public int currentPage { get; set; }
    public int countPages { get; set; }

    public async Task OnGetAsync(string SearchString)
    {

      var query = from a in _context.articles
                  select a;
      if (!string.IsNullOrEmpty(SearchString))
      {
        query = query.Where(x => x.Title.Contains(SearchString));
      }
      int totalArticle = await query.CountAsync();
      countPages = (int)Math.Ceiling((double)totalArticle / ITEMS_PER_PAGE);
      if (currentPage < 1)
        currentPage = 1;
      if (currentPage > countPages && countPages > 0)
        currentPage = countPages;

      Article = await query.OrderByDescending(x => x.Created).Skip((currentPage - 1) * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync();
    }
  }
}
