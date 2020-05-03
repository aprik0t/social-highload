
using System.ComponentModel.DataAnnotations;

namespace SocialHighload.Service.Model.Dto.Person
{
    public class DtoUpdatePerson
    {
        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Не указана фамилия")]
        [StringLength(100)]
        public string Surname { get; set; }
        
        [Required(ErrorMessage = "Укажите возраст")]
        [Range(18, 100, ErrorMessage = "Некорректный возраст для земного человека 21 века")]
        public int Age { get; set; }
        
        [Required(ErrorMessage = "Не указан город")]
        [StringLength(100)]
        public string City { get; set; }
        
        [StringLength(400, ErrorMessage = "Слишком много слов")]
        public string Bio { get; set; }
        
        [Required(ErrorMessage = "Укажите пол")]
        public int Gender { get; set; }
    }
}