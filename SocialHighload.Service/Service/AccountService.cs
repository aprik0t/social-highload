using System.Threading.Tasks;
using SocialHighload.Dal.Infrastructure.Db;
using SocialHighload.Service.Model.Dto.Account;

namespace SocialHighload.Service.Service
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

        public async Task<DtoAccount> FindByEmailAsync(string email)
        {
            var dataTable = await _dbClient.GetDataTableAsync($"SELECT * FROM Accounts WHERE `Email` = '{email}'");
            if (dataTable == null || dataTable.Rows.Count == 0)
                return null;
            
            return new DtoAccount
            {
                Email = dataTable.Rows[0]["Email"].ToString(),
                Password = dataTable.Rows[0]["Password"].ToString()
            };
        }
    }
}