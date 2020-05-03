namespace SocialHighload.Service.Model.Dto.Friend
{
    public class DtoFriendRequest
    {
        public int Id { get; set; }
        public int SenderPersonId { get; set; }
        public int ReceiverPersonId { get; set; }
    }
}