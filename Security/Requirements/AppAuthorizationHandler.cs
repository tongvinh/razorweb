using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using razorweb.models;

namespace App.Security.Requirements
{
  public class AppAuthorizationHandler : IAuthorizationHandler
  {
    private readonly ILogger<AppAuthorizationHandler> _logger;
    private readonly UserManager<AppUser> _userManager;
    public AppAuthorizationHandler(ILogger<AppAuthorizationHandler> logger, UserManager<AppUser> userManager)
    {
      _userManager = userManager;
      _logger = logger;

    }
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
      var requirements = context.PendingRequirements.ToList();
      foreach (var requirement in requirements)
      {
        if (requirement is GenZRequirement)
        {
          if (IsGenZ(context.User, (GenZRequirement)requirement))
          {
            context.Succeed(requirement);
          }
          // code xu ly kiem tra user dam bao requirement/GenZRequirement
          //context.Successed(requirement);
        }
        if (requirement is ArticleUpdateRequirement)
        {
          bool canupdate = CanUpdateArticle(context.User, context.Resource, (ArticleUpdateRequirement)requirement);
          if (canupdate)
          {
            context.Succeed(requirement);
          }
        }
      }
      return Task.CompletedTask;
    }

    private bool CanUpdateArticle(ClaimsPrincipal user, object resource, ArticleUpdateRequirement requirement)
    {
      if (user.IsInRole("Admin"))
      {
        _logger.LogInformation("Admin cập nhật");
        return true;
      }
      var article = resource as Article;
      var dateCreated = article.Created;
      var dateCanUpdate = new DateTime(requirement.Year, requirement.Month, requirement.Day);
      if (dateCreated < dateCanUpdate)
      {
        _logger.LogInformation("Quá ngày cập nhật");
        return false;
      }
      return true;
    }

    private bool IsGenZ(ClaimsPrincipal user, GenZRequirement requirement)
    {
      var appUserTask = _userManager.GetUserAsync(user);
      Task.WaitAll(appUserTask);
      var appUser = appUserTask.Result;

      if (appUser.BirthDay == null)
      {
        _logger.LogInformation($"{appUser.UserName} khong co ngay sinh, khong thoa man GenZRequirement");
        return false;
      };
      int year = appUser.BirthDay.Value.Year;

      var success = (year >= requirement.FromYear && year <= requirement.ToYear);
      if (success)
      {
        _logger.LogInformation($"{appUser.UserName} thoa man GenZRequirement");
      }
      else
      {
        _logger.LogInformation($"{appUser.UserName} khong thoa man GenZRequirement");
      }
      return success;
    }
  }
}