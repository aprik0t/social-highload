using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtusSocial.Models;
using OtusSocial.Service.Model.Dto.People;
using OtusSocial.Service.Service;

namespace OtusSocial.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IPersonService _personService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, 
            IPersonService personService, IMapper mapper)
        {
            _accountService = accountService;
            _personService = personService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View("SignIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                await Authenticate(model.Email);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            if (User.Identity.IsAuthenticated)
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
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
                return RedirectToAction("Index", "Home");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (User.Identity.IsAuthenticated) 
                return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid) 
                return View(model);
            if (await _accountService.ExistsAsync(model.Email))
                ModelState.AddModelError("Email", "Такой пользователь уже существует в базе");
            else
            {
                var personResult = await _personService.CreatePersonAsync(_mapper.Map<SignUpModel, DtoPerson>(model));
                if (personResult.HasValue)
                {
                    var accountResult = await _accountService.CreateAccountAsync(personResult.Value, model.Email, model.Password);
                    if (accountResult.HasValue)
                    {
                        await Authenticate(model.Email);
                        return RedirectToAction("Index", "Home");
                    }
                }
                    
                ModelState.AddModelError("Email", "Не удалось зарегистрировать пользователя");
            }
            return View(model);
        }
    }
}