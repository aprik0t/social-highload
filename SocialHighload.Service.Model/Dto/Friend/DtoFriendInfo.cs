using SocialHighload.Service.Model.Dto.Person;
using SocialHighload.Service.Model.Enums;

namespace SocialHighload.Service.Model.Dto.Friend
{
    public class DtoFriendInfo
    {
        public DtoPersonLite Person { get; set; }
        public int FriendRequestId { get; set; }
        public FriendRequestStatus FriendRequestStatus { get; set; }
    }
}