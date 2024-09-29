using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MVC.Helpers;
using MVC.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<Users> _userManager;
		private readonly SignInManager<Users> _signInManager;
        private readonly EmailSetting _emailSetting;

        public AccountController(UserManager<Users> userManager,SignInManager<Users> signInManager,EmailSetting emailSetting)
        {
			_userManager = userManager;
			_signInManager = signInManager;
            _emailSetting = emailSetting;
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new Users()
                {
                    FName= signUpViewModel.FName,
                    LName= signUpViewModel.LName,
                    Email= signUpViewModel.Email,
                    IsAgree= signUpViewModel.IsAgree,
                    UserName=signUpViewModel.Email.Split('@')[0],
                    PhoneNumber=signUpViewModel.PhoneNumber
                    
                };
                var Result =await _userManager.CreateAsync(user,signUpViewModel.Password);
                if (Result.Succeeded)
                {
					return RedirectToAction("SignIn");
				}
                foreach(var r in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty,r.Description);
                }
            }
            return View(signUpViewModel);
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn( SignInViewModel signInViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(signInViewModel.Email);
                if (user is not null)
                {
                    bool flag = await _userManager.CheckPasswordAsync(user, signInViewModel.Password);
                    if (flag)
                    {
                        var result=await _signInManager.PasswordSignInAsync(user, signInViewModel.Password, true, false);
                        if (result.Succeeded)
                        {
							return RedirectToAction("Index", "Home");
						}
                    }
                }
                ModelState.AddModelError(string.Empty, "Logi Erorr");
            }
            return View(signInViewModel);
        }
        //to make external login (login with google account)
        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var Result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var claims = Result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Value,
                claim.Type
            });
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn");
            
        }
		public async Task<IActionResult> ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel forgetPasswordViewModel)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
				if (user is not null)
				{

					string tok = await _userManager.GeneratePasswordResetTokenAsync(user);
					var url = Url.Action("RestPassword", "Account", new { email = forgetPasswordViewModel.Email, token = tok });
					Email email = new Email()
					{
						Subject = "Reset your Password",
						Body = url,
						Reciepent = forgetPasswordViewModel.Email
					};
					_emailSetting.SendEmail(email);
					return RedirectToAction("EmailBox");

				}
				ModelState.AddModelError(string.Empty, "not found");
			}
			return View(forgetPasswordViewModel);
		}
		public async Task<IActionResult> EmailBox()
		{
			return View();
		}

		public async Task<IActionResult> RestPassword(string email, string token)
		{
            //to sent data from action to next action (RestPassword)
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> RestPassword(RestPasswordViewModel restPasswordViewModel)
		{
			if (ModelState.IsValid)
			{

				var user = await _userManager.FindByEmailAsync(TempData["email"] as string);
				var result = await _userManager.ResetPasswordAsync(user, TempData["token"] as string, restPasswordViewModel.Password);
				if (result.Succeeded)
				{
					return RedirectToAction("SignIn");
				}
				ModelState.AddModelError(string.Empty, "not updated");
			}
			return View(restPasswordViewModel);
		}

	}
}
