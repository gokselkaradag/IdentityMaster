using IdentityApp.Data;
using IdentityApp.Models;
using IdentityApp.Models.Mail;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Security.Claims;

namespace IdentityApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private IEmailSender _emailSender;

        public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDto loginDTO)
        {
            
                var user = await _userManager.FindByNameAsync(loginDTO.Username);
                if (user == null)
                {
                    TempData["message"] = "Yanlış mail ya da e-posta";
                    return RedirectToAction("Error", "Home");
                }
                else
                {
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        TempData["message"] = "Hesabınızı Onaylayınız";
                        return RedirectToAction("Error", "Home");
                    }
                    var result = await _signInManager.PasswordSignInAsync(loginDTO.Username, loginDTO.Password, false, true);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
             
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["message"] = "Eposta adresinizi giriniz";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                TempData["message"] = "Eposta adresiyle eşleşen bir kayıt yok";
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPassword", "Login", new { user.Id, token });

            await _emailSender.SendEmailAsync(user.Email, "Parola Sıfırlama", "https://localhost:44317" + url);
            TempData["message"] = "Parolanızı sıfırlamak için mailinizi kontrol edin";
            return View();

        }

        [HttpGet]
        public IActionResult ResetPassword(string Id, string token)
        {
            if (Id == null || token == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ResetPasswordDto { Token = token };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    ViewBag.ErrorMessage = "Kullanıcı Bulunamadı";
                    return View();
                }
                var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password);

                if (result.Succeeded)
                {
                    TempData["message"] = "Şifreniz Değiştirildi";
                    return RedirectToAction("Index");
                }
            }
            return View(dto);
        }

        public IActionResult GoogleLogin(string ReturnUrl)
        {
            string redirectUrl = Url.Action("ExternalResponse", "Login", new { ReturnUrl = ReturnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            properties.Items["scope"] = "openid profile email";
            //properties.Items["prompt"] = "consent";
            return new ChallengeResult("Google", properties);
        }
        public async Task<IActionResult> ExternalResponse(string ReturnUrl = "/")

        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ViewBag.ErrorMessage = "Google hesabı ile giriş başarısız oldu.";
                return RedirectToAction("Index");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var addLoginResult = await _userManager.AddLoginAsync(user, info);
                if (addLoginResult.Succeeded || (await _userManager.GetLoginsAsync(user)).Any(l => l.LoginProvider == info.LoginProvider))
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    var resultPassword = await _userManager.HasPasswordAsync(user);
                    if (!resultPassword)
                    {
                        return RedirectToAction("SetPassword", new { userId = user.Id });
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Bu Google hesabı mevcut kullanıcıya eklenemedi.";
                    return RedirectToAction("Index");
                }
            }
            user = new AppUser
            {
                UserName = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier),
                Email = email,
                Name = info.Principal.FindFirstValue(ClaimTypes.Name),
                Surname = info.Principal.FindFirstValue(ClaimTypes.Surname),
                PhoneNumber = info.Principal.FindFirstValue(ClaimTypes.MobilePhone),
            };
            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                ViewBag.ErrorMessage = "Kullanıcı oluşturulamadı";
                return RedirectToAction("Index");
            }

            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("SetPassword", new { userId = user.Id });
        }

        public IActionResult FacebookLogin(string ReturnUrl)
        {
            string redirectUrl = Url.Action("FacebookResponse", "Login", new { ReturnUrl = ReturnUrl });
            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook", properties);
        }

        public async Task<IActionResult> FacebookResponse(string ReturnUrl = "/")
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("Index");

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var userLogins = await _userManager.GetLoginsAsync(user);
                var isAlreadyLinked = userLogins.Any(x => x.LoginProvider == info.LoginProvider);

                if (!isAlreadyLinked)
                {
                    var addLoginResult = await _userManager.AddLoginAsync(user, info);
                    if (!addLoginResult.Succeeded)
                    {
                        ViewBag.ErrorMessage = "Kullanıcıya giriş sağlayıcısı eklenemedi.";
                        return RedirectToAction("Index");
                    }

                }
                await _signInManager.SignInAsync(user, isPersistent: false);

                if (!await _userManager.HasPasswordAsync(user))
                {
                    return RedirectToAction("SetPassword", new { userId = user.Id });
                }

                return RedirectToAction("Index", "Home");
            }
            user = new AppUser
            {
                UserName = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier),
                Email = email,
                Name = info.Principal.FindFirstValue(ClaimTypes.Name),
                Surname = info.Principal.FindFirstValue(ClaimTypes.Surname),
                PhoneNumber = info.Principal.FindFirstValue(ClaimTypes.MobilePhone),
            };
            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                ViewBag.ErrorMessage = "Kullanıcı oluşturulamadı";
                return RedirectToAction("Index");
            }

            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("SetPassword", new { userId = user.Id });
        }
    }
}
