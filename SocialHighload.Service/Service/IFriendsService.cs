using System.Threading.Tasks;

namespace SocialHighload.Service.Service
{
    public interface IFriendsService
    {
        Task AddToFriendsAsync(int senderPersonId, int receiverPersonId);
        Task DeleteRequestAsync(int senderPersonId, int receiverPersonId);
    }
}