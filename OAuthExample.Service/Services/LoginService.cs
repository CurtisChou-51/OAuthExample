using Microsoft.Extensions.Logging;
using OAuthExample.Service.Clients;
using OAuthExample.Service.Entities;
using OAuthExample.Service.Models;
using OAuthExample.Service.Repositories;

namespace OAuthExample.Service.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<IOAuthService> _oAuthServices;
        private readonly ILoginRepository _loginRepository;
        private readonly IStateManageService _stateManageService;

        public LoginService(ILogger<LoginService> logger, IEnumerable<IOAuthService> oAuthServices, ILoginRepository loginRepository, IStateManageService stateManageService)
        {
            _logger = logger;
            _oAuthServices = oAuthServices;
            _loginRepository = loginRepository;
            _stateManageService = stateManageService;
        }

        /// <summary> 取得 OAuth 登入 Url </summary>
        public OAuthLoginUrlDto GetOAuthLoginUrl(string authenticationMethod)
        {
            var service = _oAuthServices.FirstOrDefault(x => x.AuthenticationMethod.ToString() == authenticationMethod);
            if (service == null)
                return new OAuthLoginUrlDto { Error = "undefined authenticationMethod" };
            string state = _stateManageService.GenerateState();
            string url = service.GetLoginPageUrl(state);
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

                if (!_stateManageService.ValidateState(state))
                    return new LoginResultDto { Error = "state is not valid" };

                LoginClientDataDto loginClientDataDto = await oAuthService.Login(code);
                LoginUserInfoDto userInfo = GetOrCreateUserInfo(loginClientDataDto);
                return new LoginResultDto { Success = true, UserInfo = userInfo };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing callback");
                return new LoginResultDto { Error = "Error processing callback" };
            }
        }

        private LoginUserInfoDto GetOrCreateUserInfo(LoginClientDataDto loginClientDataDto)
        {
            UserLoginLinkEntity userLoginLinkEntity = new UserLoginLinkEntity
            {
                ClientId = loginClientDataDto.Id,
                AuthenticationMethod = loginClientDataDto.AuthenticationMethod.ToString()
            };
            UserInfoEntity userInfoEntity = new UserInfoEntity
            {
                UserName = loginClientDataDto.Name
            };

            UserInfoEntity result = _loginRepository.GetOrCreateUserInfo(userLoginLinkEntity, userInfoEntity);
            return new LoginUserInfoDto
            {
                UserId = result.UserId,
                UserName = result.UserName
            };
        }

    }
}
