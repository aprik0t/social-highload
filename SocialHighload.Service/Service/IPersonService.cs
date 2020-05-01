using System.Collections.Generic;
using System.Threading.Tasks;
using OtusSocial.Service.Model.Dto.People;

namespace OtusSocial.Service.Service
{
    public interface IPersonService
    {
        Task<int?> CreatePersonAsync(DtoPerson person);
        Task<List<DtoPerson>> GetAllPersonsAsync();
    }
}