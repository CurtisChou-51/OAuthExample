﻿using OAuthExample.Service;
using OAuthExample.Service.Models;
using OAuthExample.Web.Models;
using OAuthExample.Web.Repositories;

namespace OAuthExample.Web.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<IOAuthService> _oAuthServices;
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILogger<LoginService> logger, IEnumerable<IOAuthService> oAuthService, ILoginRepository loginRepository)
        {
            _logger = logger;
            _oAuthServices = oAuthService;
            _loginRepository = loginRepository;
        }

        /// <summary> 取得 OAuth 登入 Url </summary>
        public OAuthLoginUrlDto GetOAuthLoginUrl(string authenticationMethod)
        {
            var service = _oAuthServices.FirstOrDefault(x => x.AuthenticationMethod.ToString() == authenticationMethod);
            if (service == null)
                return new OAuthLoginUrlDto { Error = "undefined authenticationMethod" };
            string url = service.GetLoginPageUrl();
            return new OAuthLoginUrlDto { Success = true, Url = url };
        }

        /// <summary> OAuth 登入 </summary>
        public async Task<LoginResultDto> OAuthLogin(string authenticationMethod, string code, string state)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    return new LoginResultDto { Error = "code is required" };
                var oAuthService = _oAuthServices.FirstOrDefault(x => x.AuthenticationMethod.ToString() == authenticationMethod);
                if (oAuthService == null)
                    return new LoginResultDto { Error = "undefined authenticationMethod" };

                LoginDataDto loginData = await oAuthService.Login(code);
                LoginUserInfoDto userInfo = _loginRepository.GetOrCreateUserInfo(loginData);
                return new LoginResultDto { Success = true, UserInfo = userInfo };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing callback");
                return new LoginResultDto { Error = "Error processing callback" };
            }
        }
    }
}
