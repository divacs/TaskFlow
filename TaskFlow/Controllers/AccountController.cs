using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Win32;
using TaskFlow.Models.Models;
using TaskFlow.Models.Models.Account;
using TaskFlow.Utility.Interface;

namespace TaskFlow.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailService emailService, IMemoryCache cache)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            _cache = cache;
        }

        //// GET: /Account/Login
        //[HttpGet]
        //public IActionResult LoginConfirmation()
        //{
        //    return View(); // Shows the login form
        //}

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Error", "Account");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return RedirectToAction("Error", "Account");
            }

            // Check if email is confirmed
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                // If email is not confirmed, send email again
                ModelState.AddModelError("", "You must confirm your email before logging in.");

                // Generate confirmation token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // Create confirmation link (use Request.Scheme for https/http)
                var confirmationLink = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = user.Id, token = token },
                    Request.Scheme);

                // Send mail with link
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Confirm your TaskFlow account",
                    $"Hello {user.FullName}, please confirm your account by clicking <a href='{confirmationLink}'>here</a>."
                );
                return RedirectToAction("EmailVerificationSent", "Account");

            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return RedirectToAction("Error");
            }

            // if we reach here, login was successful
            return RedirectToAction("LoginConfirmation", "Account");
        }


        // GET: /Account/LoginWelcome
        [HttpGet]
        public IActionResult LoginConfirmation()
        {
            return View(); // This will load Views/Account/Login.cshtml
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(NewUser model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            try
            {
                // Assign default role
                await _userManager.AddToRoleAsync(user, "Developer");

                // Generate confirmation token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // Create confirmation link (use Request.Scheme for https/http)
                var confirmationLink = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = user.Id, token = token },
                    Request.Scheme);

                // Send mail with link
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Confirm your TaskFlow account",
                    $"Hello {user.FullName}, please confirm your account by clicking <a href='{confirmationLink}'>here</a>."
                );

                // return page "check your email"
                return RedirectToAction("EmailVerificationSent", "Account");
            }
            catch
            {
                await _userManager.DeleteAsync(user);
                ModelState.AddModelError("", "Registration failed due to an internal error.");
                return View(model);
            }
        }
        // GET: /Account/EmailVerificationSent
        [HttpGet]
        public IActionResult EmailVerificationSent()
        {
            return View();
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Home");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("ConfirmEmailSuccess");
            }

            return View("Error");
        }
        // GET : /Account/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // POST : /Account/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Rate limiting logic (limit to 5 requests in 10 minutes)
            string key = $"forgot_{model.Email.ToLower()}"; // unique key per email

            if (_cache.TryGetValue(key, out int requestCount))
            {
                if (requestCount >= 5)
                {
                    // If limit exceeded, show error
                    return RedirectToAction("TooManyRequests");
                }

                // Increment request count and reset expiration
                _cache.Set(key, requestCount + 1, TimeSpan.FromMinutes(10));
            }
            else
            {
                // First request, set count to 1
                _cache.Set(key, 1, TimeSpan.FromMinutes(10));
            }

            // Search for user by email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // For security, do not reveal if user does not exist or is not confirmed
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            // Generate token for password reset
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Create reset link
            var resetLink = Url.Action(
                "ResetPassword",
                "Account",
                new { token, email = user.Email },
                Request.Scheme);

            // Send email with reset link
            await _emailService.SendEmailAsync(
                user.Email,
                "Password Reset",
                $"Kliknite <a href='{resetLink}'>ovde</a> da resetujete svoju lozinku."
            );

            // Redirect to confirmation page
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult TooManyRequests()
        {
            return View();
        }


        // GET: /Account/ResetPassword open form
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return RedirectToAction("Error", "Account");
            }

            var model = new ResetPassword { Token = token, Email = email };
            return View(model);
        }
        // POST: /Account/ResetPassword submit new password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }  
    }
}
