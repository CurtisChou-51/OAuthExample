using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OAuthExample.Web.Models;
using OAuthExample.Web.Services;
using System.Security.Claims;

namespace OAuthExample.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger _logger;
        private readonly ILoginService _loginService;

        public LoginController(ILogger<LoginController> logger, ILoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary> 登入頁面 </summary>
        [HttpGet]
        [Route("{controller}/{action}/{authenticationMethod}")]
        public IActionResult LoginPage(string authenticationMethod)
        {
            OAuthLoginUrlDto result = _loginService.GetOAuthLoginUrl(authenticationMethod);
            if (!result.Success)
                return BadRequest(result.Error);
            return Redirect(result.Url);
        }

        /// <summary> OAuth CallBack </summary>
        [HttpGet]
        [Route("{controller}/{action}/{authenticationMethod}")]
        public async Task<IActionResult> CallBack(string authenticationMethod, string code, string state)
        {
            LoginResultDto loginResult = await _loginService.OAuthLogin(authenticationMethod, code, state);
            if (loginResult.UserInfo == null)
                return BadRequest(loginResult.Error);
            await SetLoginCookie(loginResult.UserInfo);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("LoginAuth");
            return RedirectToAction("Index", "Home");
        }

        private async Task SetLoginCookie(LoginUserInfoDto userInfo)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.AuthenticationMethod, userInfo.AuthenticationMethod),
                new(ClaimTypes.Name, userInfo.UserName)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "LoginAuth");
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync("LoginAuth", principal, new AuthenticationProperties());
        }

    }
}
