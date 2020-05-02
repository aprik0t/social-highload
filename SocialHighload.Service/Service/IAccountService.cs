using System.Threading.Tasks;

namespace SocialHighload.Service.Service
{
    public interface IAccountService
    {
        Task<bool> ExistsAsync(string email);
        Task<int?> CreateAccountAsync(int personId, string email, string password);
    }
}