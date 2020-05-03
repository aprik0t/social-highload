using System.Collections.Generic;
using System.Threading.Tasks;
using SocialHighload.Service.Model.Dto;
using SocialHighload.Service.Model.Dto.Person;

namespace SocialHighload.Service.Service
{
    public interface IPersonService
    {
        Task<int?> CreatePersonAsync(DtoPerson person);
        Task<List<DtoPerson>> GetAllPersonsAsync(int personId);
        Task<int?> FindByLoginAsync(string email);
        Task<DtoPerson> GetPersonInfoAsync(int personId, int? curPersonId = null);
        Task<DtoPerson> UpdateAsync(int personId, DtoUpdatePerson profileData);
    }
}