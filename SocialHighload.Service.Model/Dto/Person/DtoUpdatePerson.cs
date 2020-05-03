
namespace SocialHighload.Service.Model.Dto.Person
{
    public class DtoUpdatePerson
    {
        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(100)]
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Bio { get; set; }
        public int Gender { get; set; }
    }
}