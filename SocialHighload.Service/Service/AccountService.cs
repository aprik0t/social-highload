using System.Threading.Tasks;
using OtusSocial.Dal.Infrastructure.Db;

namespace OtusSocial.Service.Service
{
    public class AccountService: IAccountService
    {
        private readonly DbClient _dbClient;

        public AccountService(DbClient dbClient)
        {
            _dbClient = dbClient;
        }
        
        public async Task<bool> ExistsAsync(string email)
        {
            var query = $"SELECT COUNT(*) FROM `Accounts` WHERE `Email` = '{email}'";
            var count = await _dbClient.TryGetIntAsync(query);
            return count > 0;
        }

        public async Task<int?> CreateAccountAsync(int personId, string email, string password)
        {
            await _dbClient.RunCmdAsync(
                "INSERT INTO `Accounts` (`Email`, `Password`, `PersonId`) " + 
                $"VALUES ('{email}', '{password}', {personId})");
            return await _dbClient.TryGetIntAsync("SELECT LAST_INSERT_ID();");
        }
    }
}