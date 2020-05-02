using System.Collections.Generic;
using System.Threading.Tasks;
using SocialHighload.Service.Model.Dto;

namespace SocialHighload.Service.Service
{
    public interface IPersonService
    {
        Task<int?> CreatePersonAsync(DtoPerson person);
        Task<List<DtoPerson>> GetAllPersonsAsync(int personId);
        Task<int?> FindByLoginAsync(string email);
    }
}