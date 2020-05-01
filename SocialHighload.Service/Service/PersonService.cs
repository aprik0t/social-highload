using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using OtusSocial.Dal.Infrastructure.Db;
using OtusSocial.Service.Model.Dto.People;

namespace OtusSocial.Service.Service
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
                "INSERT INTO `Persons`(`Surname`, `Name`, `Age`, `Gender`, `City`, `Bio`) " +
                $"VALUES ('{person.Surname}', '{person.Name}', {person.Age}, {person.Gender}, '{person.City}', '{person.Bio}');");
            return await _dbClient.TryGetIntAsync("SELECT LAST_INSERT_ID();");
        }

        public async Task<List<DtoPerson>> GetAllPersonsAsync()
        {
            var personList = new List<DtoPerson>();
            const string query = @"
                SELECT * 
                FROM 
	                Persons p 
                    LEFT JOIN Friends f ON f.Person1Id = p.Id OR f.Person2Id = p.Id";
            var dataTable = await _dbClient.GetDataTableAsync(query);
            if (dataTable == null || dataTable.Rows.Count == 0) 
                return personList;
            foreach (DataRow row in dataTable.Rows)
            {
                personList.Add(new DtoPerson
                {
                    
                });
            }

            return personList;
        }
    }
}