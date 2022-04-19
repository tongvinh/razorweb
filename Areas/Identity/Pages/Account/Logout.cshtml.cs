// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using App.Models;

namespace App.Areas.Identity.Pages.Account
{
  public class LogoutModel : PageModel
  {
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<LogoutModel> _logger;

    public LogoutModel(SignInManager<AppUser> signInManager, ILogger<LogoutModel> logger)
    {
      _signInManager = signInManager;
      _logger = logger;
    }
    public void OnGet()
    {
      // _signInManager.IsSignedIn(User);
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
      await _signInManager.SignOutAsync();
      _logger.LogInformation("User logged out.");
      if (returnUrl != null)
      {
        return LocalRedirect(returnUrl);
      }
      else
      {

        returnUrl = Url.Content("~/");
        return LocalRedirect(returnUrl);

      }
    }
  }
}
