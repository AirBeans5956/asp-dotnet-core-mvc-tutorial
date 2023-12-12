namespace MvcMovie.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous]
public class AccountController : Controller
{
    //ログイン画面表示用
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    //ログイン画面からのPOST処理用
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string loginId, string password)
    {
        // ID,PWに値が入っていれば認証を通す
        if (string.IsNullOrWhiteSpace(loginId) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError(string.Empty, "ログイン情報に誤りがあります。");
            return View("Index");
        }

        var claims = new[] {
            new Claim(ClaimTypes.Name, loginId),
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(principal);

        // 認証されたらHomeページへリダイレクトする
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction(nameof(Login));
    }
}
