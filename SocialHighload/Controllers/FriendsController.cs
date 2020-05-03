using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialHighload.Exceptions;
using SocialHighload.Service.Service;

namespace SocialHighload.Controllers
{
    [Authorize]
    public class FriendsController : Controller
    {
        private readonly IFriendsService _friendsService;
        private readonly IPersonService _personService;
        private const string AccessDenied = "Ошибка прав доступа";

        public FriendsController(IFriendsService friendsService, 
            IPersonService personService)
        {
            _friendsService = friendsService;
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var personId = await _personService.FindByLoginAsync(User.Identity.Name);
            if (!personId.HasValue)
                throw new UnknownUserException();

            var friendsInfo = await _friendsService.GetFriendsInfo(personId.Value);
            return View(friendsInfo);
        }

        [HttpGet]
        public async Task<IActionResult> SendFriendRequest(int targetPersonId, string returnUrl)
        {
            var personId = await _personService.FindByLoginAsync(User.Identity.Name);
            if (!personId.HasValue)
                throw new UnknownUserException();

            await _friendsService.SendFriendRequestAsync(personId.Value, targetPersonId);
            
            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction("Index", "Person");
            return Redirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> CancelRequest(int targetPersonId, string returnUrl)
        {
            var personId = await _personService.FindByLoginAsync(User.Identity.Name);
            if (!personId.HasValue)
                throw new UnknownUserException();

            await _friendsService.DeleteFriendRequestAsync(personId.Value, targetPersonId);
            
            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction("Index");

            return Redirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Approve(int requestId)
        {
            var personId = await _personService.FindByLoginAsync(User.Identity.Name);
            if (!personId.HasValue)
                throw new UnknownUserException();
            var friendRequestInfo = await _friendsService.GetRequestInfoAsync(requestId);
            if (friendRequestInfo.ReceiverPersonId != personId.Value)
                return BadRequest(AccessDenied);

            await _friendsService.ApproveAsync(requestId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Reject(int requestId)
        {
            var personId = await _personService.FindByLoginAsync(User.Identity.Name);
            if (!personId.HasValue)
                throw new UnknownUserException();
            var friendRequestInfo = await _friendsService.GetRequestInfoAsync(requestId);
            if (friendRequestInfo.ReceiverPersonId != personId.Value)
                return BadRequest(AccessDenied);

            await _friendsService.RejectAsync(requestId);
            return RedirectToAction("Index");
        }
    }
}