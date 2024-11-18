using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OAuthExample.Service;
using System.Security.Claims;

namespace OAuthExample.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<IOAuthService> _loginServices;

        public LoginController(ILogger<LoginController> logger, IEnumerable<IOAuthService> loginServices)
        {
            _logger = logger;
            _loginServices = loginServices;
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
            var service = _loginServices.FirstOrDefault(x => x.AuthenticationMethod.ToString() == authenticationMethod);
            if (service == null)
                return BadRequest("undefined authenticationMethod");
            string url = service.GetLoginPageUrl();
            return Redirect(url);
        }

        /// <summary> OAuth CallBack </summary>
        [HttpGet]
        [Route("{controller}/{action}/{authenticationMethod}")]
        public async Task<IActionResult> CallBack(string authenticationMethod, string code, string state)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    return BadRequest("code is required");
                var service = _loginServices.FirstOrDefault(x => x.AuthenticationMethod.ToString() == authenticationMethod);
                if (service == null)
                    return BadRequest("undefined authenticationMethod");
                List<Claim> claims = await service.Login(code);
                await SetLoginCookie(claims);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing callback");
                return BadRequest("Error processing callback");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("LoginAuth");
            return RedirectToAction("Index", "Home");
        }

        private async Task SetLoginCookie(IEnumerable<Claim> claims)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "LoginAuth");
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync("LoginAuth", principal, new AuthenticationProperties());
        }

    }
}
