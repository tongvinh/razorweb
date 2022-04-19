using Microsoft.AspNetCore.Authorization;

namespace App.Security.Requirements
{
  public class ArticleUpdateRequirement : IAuthorizationRequirement
  {
    public ArticleUpdateRequirement(int year = 2022, int month = 6, int day = 1)
    {
      Year = year;
      Month = month;
      Day = day;
    }

    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
  }
}