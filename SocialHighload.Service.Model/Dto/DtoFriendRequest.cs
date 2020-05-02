using SocialHighload.Service.Model.Enums;

namespace SocialHighload.Service.Model.Dto
{
    public class DtoFriendRequest
    {
        public int Id { get; set; }
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public FriendRequestStatus Status { get; set; }
    }
}