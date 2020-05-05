using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialHighload.Models;
using SocialHighload.Security;
using SocialHighload.Service.Model.Dto.Person;
using SocialHighload.Service.Service;

namespace SocialHighload.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IPersonService _personService;
        private readonly IMapper _mapper;
        private readonly Encrypt _encrypt;

        public AccountController(IAccountService accountService, 
            IPersonService personService, 
            IMapper mapper, 
            Encrypt encrypt)
        {
            _accountService = accountService;
            _personService = personService;
            _mapper = mapper;
            _encrypt = encrypt;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Person");
            return View("SignIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (!ModelState.IsValid) 
                return View(model);
            var person = await _personService.FindByEmailAsync(model.Email);
            if (person.HasValue)
            {
                var account = await _accountService.FindByEmailAsync(model.Email);
                if (account != null && _encrypt.VerifyHashedPassword(account.Password, model.Password))
                {
                    await Authenticate(model.Email);
                    return RedirectToAction("Index", "Person");
                }
            }
                
            ModelState.AddModelError("Password", "Неверные данные пользователя");
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            if (User.Identity.IsAuthenticated)
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("SignIn", "Account");
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated) 
                return RedirectToAction("Index", "Person");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (User.Identity.IsAuthenticated) 
                return RedirectToAction("Index", "Person");
            if (!ModelState.IsValid) 
                return View(model);
            if (await _accountService.ExistsAsync(model.Email))
                ModelState.AddModelError("Email", "Такой пользователь уже существует в базе");
            else
            {
                var personResult = await _personService.CreatePersonAsync(_mapper.Map<SignUpModel, DtoPerson>(model));
                if (personResult.HasValue)
                {
                    var hashedPassword = _encrypt.HashPassword(model.Password);
                    var accountResult = await _accountService.CreateAccountAsync(personResult.Value, model.Email, hashedPassword);
                    if (accountResult.HasValue)
                    {
                        await Authenticate(model.Email);
                        return RedirectToAction("Index", "Person");
                    }
                }
                    
                ModelState.AddModelError("Email", "Не удалось зарегистрировать пользователя");
            }
            return View(model);
        }
    }
}