using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using SocialHighload.Dal.Infrastructure.Db;
using SocialHighload.Service.Model.Dto;
using SocialHighload.Service.Model.Enums;

namespace SocialHighload.Service.Service
{
    public class PersonService: IPersonService
    {
        private readonly DbClient _dbClient;

        public PersonService(DbClient dbClient)
        {
            _dbClient = dbClient;
        }

        public async Task<int?> CreatePersonAsync(DtoPerson person)
        {
            await _dbClient.RunCmdAsync(
                $"INSERT INTO `{DbClient.PersonTable}`(`Surname`, `Name`, `Age`, `Gender`, `City`, `Bio`) " +
                $"VALUES ('{person.Surname}', '{person.Name}', {person.Age}, {person.Gender:D}, '{person.City}', '{person.Bio}');");
            return await _dbClient.TryGetIntAsync("SELECT LAST_INSERT_ID();");
        }

        public async Task<List<DtoPerson>> GetAllPersonsAsync(int personId)
        {
            _dbClient.RunCmd("DROP TABLE IF EXISTS MyFriends");
            var personList = new List<DtoPerson>();
            var createQuery = $@"
                CREATE TEMPORARY TABLE MyFriends
                SELECT DISTINCT (FriendId), `Status` FROM
	                (SELECT SenderPersonId AS FriendId, `Status` FROM {DbClient.FriendsTable} WHERE ReceiverPersonId = {personId}
	                UNION ALL
	                SELECT ReceiverPersonId AS FriendId, `Status` FROM {DbClient.FriendsTable} WHERE SenderPersonId = {personId}) AS TMP;
            ";
            _dbClient.RunCmd(createQuery);
            var selectQuery = $@"
                SELECT p.*, f.Status
                FROM 
	                Persons p
                    LEFT JOIN MyFriends f ON f.FriendId = p.Id
                WHERE 
	                p.Id <> {personId}";
            var dataTable = await _dbClient.GetDataTableAsync(selectQuery);
            if (dataTable == null || dataTable.Rows.Count == 0) 
                return personList;
            foreach (DataRow row in dataTable.Rows)
            {
                personList.Add(new DtoPerson
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Surname = row["Surname"].ToString(),
                    Name = row["Name"].ToString(),
                    Age = Convert.ToInt32(row["Age"]),
                    Gender = Enum.Parse<Gender>(row["Gender"].ToString()),
                    Bio = row["Bio"].ToString(),
                    City = row["City"].ToString(),
                    Status = Convert.IsDBNull(row["Status"]) 
                        ? FriendRequestStatus.None  
                        : Enum.Parse<FriendRequestStatus>(row["Status"].ToString())
                });
            }

            _dbClient.RunCmd("DROP TABLE MyFriends");

            return personList;
        }

        public async Task<int?> FindByLoginAsync(string email)
        {
            var query = $@"
            SELECT p.Id
            FROM
	            Persons p 
                LEFT JOIN Accounts a ON a.PersonId = p.Id
            WHERE
	            a.Email = '{email}'";
            var dataTable = await _dbClient.GetDataTableAsync(query);
            if (dataTable == null || dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return Convert.ToInt32(row["Id"]);
        }
    }
}