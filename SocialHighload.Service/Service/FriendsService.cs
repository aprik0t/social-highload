using System.Threading.Tasks;
using SocialHighload.Dal.Infrastructure.Db;
using SocialHighload.Service.Model.Enums;

namespace SocialHighload.Service.Service
{
    public class FriendsService: IFriendsService
    {
        private readonly DbClient _dbClient;

        public FriendsService(DbClient dbClient)
        {
            _dbClient = dbClient;
        }

        public Task AddToFriendsAsync(int senderPersonId, int receiverPersonId)
        {
            var query = $"INSERT INTO {DbClient.FriendsTable} (`SenderPersonId`, `ReceiverPersonId`, `Status`) " +
                        $"VALUES ({senderPersonId}, {receiverPersonId}, {FriendRequestStatus.Sent:D})";
            return _dbClient.RunCmdAsync(query);
        }

        public Task DeleteRequestAsync(int senderPersonId, int receiverPersonId)
        {
            var query = $"DELETE FROM {DbClient.FriendsTable} " +
                        "WHERE " +
                        $"`SenderPersonId` = {senderPersonId} AND `ReceiverPersonId` = {receiverPersonId}" +
                        $" OR `SenderPersonId` = {receiverPersonId} AND `ReceiverPersonId` = {senderPersonId}";
            return _dbClient.RunCmdAsync(query);
        }
    }
}