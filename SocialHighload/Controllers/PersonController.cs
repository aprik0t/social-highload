using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialHighload.Exceptions;
using SocialHighload.Service.Model.Dto.Person;
using SocialHighload.Service.Service;

namespace SocialHighload.Controllers
{
    [Authorize]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;
        private readonly IMapper _mapper;

        public PersonController(IPersonService personService, 
            IMapper mapper)
        {
            _personService = personService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var personId = await _personService.FindByLoginAsync(User.Identity.Name);
            if (!personId.HasValue) 
                throw new UnknownUserException();
            var persons = await _personService.GetAllPersonsAsync(personId.Value);
            
            return View(persons);
        }

        [HttpGet]
        public async Task<IActionResult> Person(int personId)
        {
            var curPersonId = await _personService.FindByLoginAsync(User.Identity.Name);
            if (!curPersonId.HasValue) 
                throw new UnknownUserException();
            var personInfo = await _personService.GetPersonInfoAsync(personId, curPersonId);
            if (personInfo == null)
                throw new UnknownPersonException();
            
            return View(personInfo);
        }
        
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var curPersonId = await _personService.FindByLoginAsync(User.Identity.Name);
            if (!curPersonId.HasValue) 
                throw new UnknownUserException();
            var personInfo = await _personService.GetPersonInfoAsync(curPersonId.Value);
            
            return View(_mapper.Map<DtoUpdatePerson>(personInfo));
        }

        [HttpPost]
        public async Task<IActionResult> Profile(DtoUpdatePerson profileData)
        {
            var curPersonId = await _personService.FindByLoginAsync(User.Identity.Name);
            if (!curPersonId.HasValue) 
                throw new UnknownUserException();
            var updatedPerson = await _personService.UpdateAsync(curPersonId.Value, profileData);

            TempData["SuccessMessage"] = "Профиль успешно сохранен";
            return View(_mapper.Map<DtoUpdatePerson>(updatedPerson));
        }
    }
}