# OAuthExample

OAuthExample 提供 OAuth 登入功能的範例

## 專案結構

- `OAuthExample.Web`：包含基本的的網站頁面
- `OAuthExample.Service`：包含 OAuth 服務的介面和實作

## 主要功能

### LoginController

`LoginController` 主要為網站提供登入頁面與對外 CallBack 接口，與 LoginService 對接

- `LoginPage`：此方法供 Razor 頁面連結使用，取得對應的登入頁面 URL
- `CallBack`：此方法供 Oauth 登入後 CallBack 呼叫接口
- 以及其他網頁相關功能

### LoginService

`LoginService` 介於 `IOAuthService` 與展示層之間，呼叫對應的的 `IOAuthService` instance 進行登入處理

- `GetOAuthLoginUrl`：根據 authenticationMethod 取得對應的登入頁面 URL
- `OAuthLogin`：根據 authenticationMethod 與 code 進行登入，並將取得的資料轉換為系統內使用者資料
- 對 Repository 存取使用者資料
- 透過 StateService 管理 state 防止 CSRF 攻擊

### IOAuthService

`IOAuthService` 介面定義了 OAuth 服務的基本功能，此範例有 Google 與 Line 實作

- `AuthenticationMethod`：驗證方法
- `GetLoginPageUrl()`：取得登入頁面網址
- `Login(string code)`：根據 code 進行登入
