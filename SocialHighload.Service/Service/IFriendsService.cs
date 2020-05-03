using System.Threading.Tasks;
using SocialHighload.Service.Model.Dto.Friend;

namespace SocialHighload.Service.Service
{
    public interface IFriendsService
    {
        Task SendFriendRequestAsync(int senderPersonId, int receiverPersonId);
        Task DeleteFriendRequestAsync(int senderPersonId, int receiverPersonId);
        Task<DtoFriendsInfo> GetFriendsInfo(int personId);
        Task<DtoFriendRequest> GetRequestInfoAsync(int requestId);
        Task RejectAsync(int requestId);
        Task ApproveAsync(int requestId);
    }
}