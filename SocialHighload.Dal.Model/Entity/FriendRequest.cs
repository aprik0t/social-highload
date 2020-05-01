using System;
using OtusSocial.Dal.Model.Enum;

namespace OtusSocial.Dal.Model.Entity
{
    public class FriendRequest
    {
        public Guid Id { get; set; }
        public Guid SenderUserId { get; set; }
        public Guid ReceiverUserId { get; set; }
        public FriendRequestStatus Status { get; set; }
    }
}