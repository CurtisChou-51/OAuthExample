using OAuthExample.Service.Models;
using OAuthExample.Service.Services;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace OAuthExample.AppClient
{
    public partial class Form1 : Form
    {
        private HttpListener _httpListener;
        private readonly ILoginService _loginService;

        public Form1(ILoginService loginService)
        {
            InitializeComponent();
            _loginService = loginService;
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add("http://localhost:7070/Login/CallBack/");
            _httpListener.Start();
        }

        private void btnGoogle_Click(object sender, EventArgs e)
        {
            LoginButtonsClick("Google");
        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            LoginButtonsClick("Line");
        }

        private void btnGithub_Click(object sender, EventArgs e)
        {
            LoginButtonsClick("Github");
        }

        private void btnMicrosoft_Click(object sender, EventArgs e)
        {
            LoginButtonsClick("Microsoft");
        }

        private void LoginButtonsClick(string authenticationMethod)
        {
            _ = OAuthCallBack();
            OpenBrowserLoginPage(authenticationMethod);
        }

        /// <summary> 使用瀏覽器開啟登入頁面 </summary>
        private void OpenBrowserLoginPage(string authenticationMethod)
        {
            OAuthLoginUrlDto result = _loginService.GetOAuthLoginUrl(authenticationMethod);
            if (!result.Success)
                return;
            using Process browserProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = result.Url,
                    UseShellExecute = true,
                    Verb = ""
                }
            };
            browserProcess.Start();
        }

        private async Task OAuthCallBack()
        {
            HttpListenerContext context = await _httpListener.GetContextAsync();
            LoginResultDto loginResult = await OAuthLoginAsync(context.Request);
            using HttpListenerResponse response = context.Response;
            if (loginResult.UserInfo == null)
                await ShowFailureResultAsync(loginResult.Error, response);
            else
                await ShowSuccessResultAsync(loginResult.UserInfo, response);
        }

        private static async Task ShowFailureResultAsync(string? error, HttpListenerResponse response)
        {
            await WriteHtmlResponse(response, "登入失敗");
            MessageBox.Show(error);
        }

        private static async Task ShowSuccessResultAsync(LoginUserInfoDto userInfo, HttpListenerResponse response)
        {
            await WriteHtmlResponse(response, "登入成功！您可以關閉此頁面。");
            MessageBox.Show($"Welcome {userInfo.UserName}");
        }

        private async Task<LoginResultDto> OAuthLoginAsync(HttpListenerRequest callbackReq)
        {
            string code = callbackReq.QueryString.Get("code") ?? string.Empty;
            string state = callbackReq.QueryString.Get("state") ?? string.Empty;
            Match match = Regex.Match(callbackReq.Url?.AbsolutePath ?? string.Empty, @"^/Login/CallBack/(.*)$", RegexOptions.IgnoreCase);
            string authenticationMethod = match.Groups[1].Value;
            return await _loginService.OAuthLogin(authenticationMethod, code, state);
        }

        private static Task WriteHtmlResponse(HttpListenerResponse callbackResp, string text)
        {
            string content = $"<html><meta charset=\"utf-8\"><head></head><body>{text}</body></html>";
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            callbackResp.ContentLength64 = buffer.Length;
            return callbackResp.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}
