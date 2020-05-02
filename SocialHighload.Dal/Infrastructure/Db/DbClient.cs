using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SocialHighload.Dal.Infrastructure.Db
{
    public class DbClient
    {
        private readonly string _connectionString;
        public const string PersonTable = "Persons";
        public const string FriendsTable = "FriendRequests";
        public const string AccountsTable = "Accounts";

        public DbClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        private async Task<MySqlConnection> GetSqlConnectionAsync()
        {
            var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
        
        public async Task<DataTable> GetDataTableAsync(string query)
        {
            using (var connection = await GetSqlConnectionAsync())
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = query;
                var dataReader = await cmd.ExecuteReaderAsync();
                var dataTable = new DataTable();
                dataTable.Load(dataReader);
                return dataTable;
            }
        }

        public void RunCmd(string query)
        {
            using (var connection = GetSqlConnectionAsync().GetAwaiter().GetResult())
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
        }

        public async Task RunCmdAsync(string query)
        {
            using (var connection = await GetSqlConnectionAsync())
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = query;
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task<bool> TablesCreated()
        {
            var tableList = new List<string>();
            using (var connection = await GetSqlConnectionAsync())
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SHOW TABLES";
                var dataReader = await cmd.ExecuteReaderAsync();
                
                while (await dataReader.ReadAsync())
                    tableList.Add(dataReader[0].ToString().ToUpper());
            }

            return tableList.OrderBy(t => t).SequenceEqual(new[]
            {
                AccountsTable.ToUpper(), 
                FriendsTable.ToUpper(),
                PersonTable.ToUpper()
            });
        }
        
        public async Task EnsureDbCreated()
        {
            if (await TablesCreated())
                return;
            
            var query =
                #region SQL для создания базовых таблиц
                $@"
                    DROP TABLE IF EXISTS `{FriendsTable}`;
                    DROP TABLE IF EXISTS `{AccountsTable}`;
                    DROP TABLE IF EXISTS `{PersonTable}`;
                    
                    CREATE TABLE `{PersonTable}` (
                      `Id` int unsigned NOT NULL AUTO_INCREMENT ,
                      `Surname` varchar(100) NOT NULL,
                      `Name` varchar(100) NOT NULL,
                      `Age` smallint unsigned NOT NULL,
                      `Gender` smallint unsigned NOT NULL,
                      `City` varchar(100) NOT NULL,
                      `Bio` text,
                      PRIMARY KEY (`Id`)
                    );

                    CREATE TABLE `{FriendsTable}` (
                      `Id` int NOT NULL AUTO_INCREMENT,
                      `SenderPersonId` int unsigned NOT NULL,
                      `ReceiverPersonId` int unsigned NOT NULL,
                      `Status` int unsigned NOT NULL DEFAULT 0,
                      PRIMARY KEY (`Id`),
                      KEY `SenderPersonId` (`SenderPersonId`),
                      KEY `ReceiverPersonId` (`ReceiverPersonId`),
                      CONSTRAINT `Friends_ibfk_1` FOREIGN KEY (`SenderPersonId`) REFERENCES `Persons` (`Id`) ON DELETE CASCADE,
                      CONSTRAINT `Friends_ibfk_2` FOREIGN KEY (`ReceiverPersonId`) REFERENCES `Persons` (`Id`) ON DELETE CASCADE,
                      CONSTRAINT `Friends_relation_unique` UNIQUE KEY (`SenderPersonId`, `ReceiverPersonId`),
                      CONSTRAINT `Friends_relation_unique_reverse` UNIQUE KEY (`ReceiverPersonId`, `SenderPersonId`)
                    );

                    CREATE TABLE `{AccountsTable}` (
                      `Id` int NOT NULL AUTO_INCREMENT ,
                      `Email` varchar(100) NOT NULL,
                      `Password` varchar(100) NOT NULL,
                      `PersonId` int unsigned NOT NULL,
                      PRIMARY KEY (`Id`),
                      KEY `PersonId` (`PersonId`),
                      CONSTRAINT `Accounts_ibfk_1` FOREIGN KEY (`PersonId`) REFERENCES `Persons` (`Id`) ON DELETE CASCADE
                    );";
            #endregion

            await RunCmdAsync(query);
        }
        
        public async Task<int?> TryGetIntAsync(string query)
        {
            using (var connection = await GetSqlConnectionAsync())
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = query;
                if (int.TryParse((await cmd.ExecuteScalarAsync()).ToString(), out var result))
                    return result;
                return null;
            }
        }
    }
}