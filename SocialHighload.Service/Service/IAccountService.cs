using System.Threading.Tasks;
using SocialHighload.Service.Model.Dto.Account;

namespace SocialHighload.Service.Service
{
    public interface IAccountService
    {
        Task<bool> ExistsAsync(string email);
        Task<int?> CreateAccountAsync(int personId, string email, string password);
        Task<DtoAccount> FindByEmailAsync(string email);
    }
}