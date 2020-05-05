using System.Collections.Generic;
using System.Threading.Tasks;
using SocialHighload.Service.Model.Dto.Person;

namespace SocialHighload.Service.Service
{
    public interface IPersonService
    {
        Task<int?> CreatePersonAsync(DtoPerson person);
        Task<List<DtoPerson>> GetAllPersonsAsync(int currentPersonId);
        Task<int?> FindByEmailAsync(string email);
        Task<DtoPerson> GetPersonInfoAsync(int personId, int? currentPersonId = null);
        Task<DtoPerson> UpdateAsync(int personId, DtoUpdatePerson profileData);
    }
}