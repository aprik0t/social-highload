using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using OtusSocial.Dal.Model.Entity;

namespace OtusSocial.Dal.Infrastructure.Db
{
    public class DbClient
    {
        private readonly string _connectionString;

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
                {
                    tableList.Add(dataReader[0].ToString());
                }
            }

            return tableList.OrderBy(t => t).SequenceEqual(new[] {"accounts", "friends", "persons"});
        }
        
        public async Task EnsureDbCreated()
        {
            if (await TablesCreated())
                return;
            
            var query =
                #region SQL для создания базовых таблиц
                @"
                    DROP TABLE IF EXISTS `Persons`;
                    CREATE TABLE `Persons` (
                      `Id` int unsigned NOT NULL AUTO_INCREMENT ,
                      `Surname` varchar(100) NOT NULL,
                      `Name` varchar(100) NOT NULL,
                      `Age` smallint unsigned NOT NULL,
                      `Gender` smallint unsigned NOT NULL,
                      `City` varchar(100) NOT NULL,
                      `Bio` text,
                      PRIMARY KEY (`Id`)
                    );

                    DROP TABLE IF EXISTS `Friends`;
                    CREATE TABLE `Friends` (
                      `Id` int NOT NULL AUTO_INCREMENT,
                      `Person1Id` int unsigned NOT NULL,
                      `Person2Id` int unsigned NOT NULL,
                      PRIMARY KEY (`Id`),
                      KEY `Person1Id` (`Person1Id`),
                      KEY `Person2Id` (`Person2Id`),
                      CONSTRAINT `Friends_ibfk_1` FOREIGN KEY (`Person1Id`) REFERENCES `Persons` (`Id`) ON DELETE CASCADE,
                      CONSTRAINT `Friends_ibfk_2` FOREIGN KEY (`Person2Id`) REFERENCES `Persons` (`Id`) ON DELETE CASCADE
                    );

                    DROP TABLE IF EXISTS `Accounts`;
                    CREATE TABLE `Accounts` (
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