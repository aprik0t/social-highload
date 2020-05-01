using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtusSocial.Service.Service;

namespace OtusSocial.Controllers
{
    [Authorize]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> Network()
        {
            var persons = await _personService.GetAllPersonsAsync();
            
            return View(persons);
        }
    }
}