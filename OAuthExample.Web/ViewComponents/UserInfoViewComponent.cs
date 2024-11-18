using Microsoft.AspNetCore.Mvc;
using OAuthExample.Web.Models;

public class UserInfoViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var user = HttpContext.User;
        var model = new UserInfoViewModel
        {
            UserName = user?.Identity?.Name ?? string.Empty,
            IsAuthenticated = user?.Identity?.IsAuthenticated ?? false
        };
        return View(model);
    }
}