using System.Collections.Generic;

namespace SocialHighload.Service.Model.Dto.Friend
{
    public class DtoFriendsInfo
    {
        public List<DtoFriendInfo> Friends { get; }
        public List<DtoFriendInfo> IncomingRequests { get; }
        public List<DtoFriendInfo> OutgoingRequests { get; }

        public DtoFriendsInfo()
        {
            Friends = new List<DtoFriendInfo>();
            IncomingRequests = new List<DtoFriendInfo>();
            OutgoingRequests = new List<DtoFriendInfo>();
        }
    }
}