using SocialHighload.Service.Model.Enums;

namespace SocialHighload.Service.Model.Dto.Person
{
    public class DtoPerson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public Gender Gender { get; set; }
        public string Bio { get; set; }
        public FriendRequestStatus Status { get; set; }
    }
}