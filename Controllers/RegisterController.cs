using IdentityApp.Data;
using IdentityApp.Models;
using IdentityApp.Models.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private IEmailSender _emailSender;
        

        public RegisterController(UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterDto userDTO)
        {
            if (userDTO.Password != userDTO.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Şifreler eşleşmiyor.");
                return View(userDTO);
            }

            AppUser user = new AppUser()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Surname = userDTO.Surname,
                UserName = userDTO.Username,
                
            };
            var result = await _userManager.CreateAsync(user, userDTO.Password);


            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Register", new { user.Id, token });

                await _emailSender.SendEmailAsync(user.Email, "Hesap Onayı", "https://localhost:44317" + url);
                TempData["message"] = "Hesap onayı için mailinizi kontrol edin";
                return RedirectToAction("Index", "Login");
            }
            return View(userDTO);
        }

        public async Task<IActionResult> ConfirmEmail(string Id, string token)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                TempData["message"] = "Geçersiz Token Bilgisi";
                return RedirectToAction("Error", "Home");
            }

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    TempData["message"] = "Mail Onaylandı";
                    ViewData["Title"] = "Confirm Email";
                    return View();
                }
                else
                {
                    TempData["message"] = "Mail Onaylanmadı";
                    return RedirectToAction("Error", "Home");
                }
            }

            return View();
        }
    }
}
