using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialHighload.Exceptions;
using SocialHighload.Service.Service;

namespace SocialHighload.Controllers
{
    [Authorize]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;
        private readonly IFriendsService _friendsService;

        public PersonController(IPersonService personService,
            IFriendsService friendsService)
        {
            _personService = personService;
            _friendsService = friendsService;
        }

        [HttpGet]
        public async Task<IActionResult> People()
        {
            var personId = await _personService.FindByLoginAsync(User.Identity.Name);
            if (!personId.HasValue) 
                throw new UnknownUserException();
            var persons = await _personService.GetAllPersonsAsync(personId.Value);
            
            return View(persons);
        }

        [HttpGet]
        public async Task<IActionResult> AddToFriends(int targetPersonId)
        {
            var personId = await _personService.FindByLoginAsync(User.Identity.Name);
            if (!personId.HasValue)
                throw new UnknownUserException();

            await _friendsService.AddToFriendsAsync(personId.Value, targetPersonId);
            
            return RedirectToAction("People");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRequest(int targetPersonId)
        {
            var personId = await _personService.FindByLoginAsync(User.Identity.Name);
            if (!personId.HasValue)
                throw new UnknownUserException();

            await _friendsService.DeleteRequestAsync(personId.Value, targetPersonId);
            
            return RedirectToAction("People");
        }
    }
}