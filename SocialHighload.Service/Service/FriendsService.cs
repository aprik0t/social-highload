using System;
using System.Data;
using System.Threading.Tasks;
using SocialHighload.Dal.Infrastructure.Db;
using SocialHighload.Service.Model.Dto.Friend;
using SocialHighload.Service.Model.Dto.Person;
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

        public Task SendFriendRequestAsync(int senderPersonId, int receiverPersonId)
        {
            var query = $"INSERT INTO {DbClient.FriendsTable} (`SenderPersonId`, `ReceiverPersonId`, `Status`) " +
                        $"VALUES ({senderPersonId}, {receiverPersonId}, {FriendRequestStatus.Sent:D})";
            return _dbClient.RunCmdAsync(query);
        }

        public Task DeleteFriendRequestAsync(int senderPersonId, int receiverPersonId)
        {
            var query = $"DELETE FROM {DbClient.FriendsTable} " +
                        "WHERE " +
                        $"`SenderPersonId` = {senderPersonId} AND `ReceiverPersonId` = {receiverPersonId}" +
                        $" OR `SenderPersonId` = {receiverPersonId} AND `ReceiverPersonId` = {senderPersonId}";
            return _dbClient.RunCmdAsync(query);
        }

        public async Task<DtoFriendsInfo> GetFriendsInfo(int personId)
        {
            var friendsInfo = new DtoFriendsInfo();
            var dataSet = await _dbClient.GetDataSetASync(
        $@"SELECT f.Id AS RequestId, f.Status, p.* FROM {DbClient.FriendsTable} f LEFT JOIN {DbClient.PersonsTable} p ON p.Id = f.SenderPersonId WHERE f.ReceiverPersonId = {personId} AND `Status` <> {FriendRequestStatus.Approved:D};
                SELECT f.Id AS RequestId, f.Status, p.* FROM {DbClient.FriendsTable} f LEFT JOIN {DbClient.PersonsTable} p ON p.Id = f.ReceiverPersonId WHERE f.SenderPersonId = {personId} AND `Status` <> {FriendRequestStatus.Approved:D};
                SELECT f.Id as RequestId, p.*
                FROM 
	                {DbClient.FriendsTable} f 
                    LEFT JOIN {DbClient.PersonsTable} p ON p.Id = f.ReceiverPersonId
                WHERE 
	                f.SenderPersonId = {personId} AND f.`Status` = {FriendRequestStatus.Approved:D}
                UNION ALL
                SELECT f.Id as RequestId, p.*
                FROM 
	                {DbClient.FriendsTable} f 
                    LEFT JOIN {DbClient.PersonsTable} p ON p.Id = f.SenderPersonId
                WHERE 
	                f.ReceiverPersonId = {personId} AND f.`Status` = {FriendRequestStatus.Approved:D};"
                );
            var incomingData = dataSet.Tables[0];
            if (incomingData.Rows.Count > 0)
            {
                foreach (DataRow row in incomingData.Rows)
                {
                    friendsInfo.IncomingRequests.Add(new DtoFriendInfo
                    {
                        FriendRequestId = Convert.ToInt32(row["RequestId"]),
                        FriendRequestStatus = Enum.Parse<FriendRequestStatus>(row["Status"].ToString()),
                        Person = new DtoPersonLite
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Name = $"{row["Surname"]} {row["Name"]}",
                            Age = Convert.ToInt32(row["Age"]),
                            City = row["City"].ToString()
                        }
                    });
                }
            }
            var outcomingData = dataSet.Tables[1];
            if (outcomingData.Rows.Count > 0)
            {
                foreach (DataRow row in outcomingData.Rows)
                {
                    friendsInfo.OutgoingRequests.Add(new DtoFriendInfo
                    {
                        FriendRequestId = Convert.ToInt32(row["RequestId"]),
                        FriendRequestStatus = Enum.Parse<FriendRequestStatus>(row["Status"].ToString()),
                        Person = new DtoPersonLite
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Name = $"{row["Surname"]} {row["Name"]}",
                            Age = Convert.ToInt32(row["Age"]),
                            City = row["City"].ToString()
                        }
                    });
                }
            }
            var friendsData = dataSet.Tables[2];
            if (friendsData.Rows.Count > 0)
            {
                foreach (DataRow row in friendsData.Rows)
                {
                    friendsInfo.Friends.Add(new DtoFriendInfo
                    {
                        FriendRequestId = Convert.ToInt32(row["RequestId"]),
                        FriendRequestStatus = FriendRequestStatus.Approved,
                        Person = new DtoPersonLite
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Name = $"{row["Surname"]} {row["Name"]}",
                            Age = Convert.ToInt32(row["Age"]),
                            City = row["City"].ToString()
                        }
                    });
                }
            }

            return friendsInfo;
        }

        public async Task<DtoFriendRequest> GetRequestInfoAsync(int requestId)
        {
            var query = $"SELECT * FROM {DbClient.FriendsTable} WHERE Id = {requestId}";
            var dataTable = await _dbClient.GetDataTableAsync(query);
            return dataTable != null && dataTable.Rows.Count > 0
                ? new DtoFriendRequest
                {
                    Id = Convert.ToInt32(dataTable.Rows[0]["Id"]),
                    ReceiverPersonId = Convert.ToInt32(dataTable.Rows[0]["ReceiverPersonId"]),
                    SenderPersonId = Convert.ToInt32(dataTable.Rows[0]["SenderPersonId"])
                }
                : null;
        }

        public Task RejectAsync(int requestId)
        {
            var query = $"UPDATE {DbClient.FriendsTable} SET `Status` = {FriendRequestStatus.Rejected:D} WHERE Id = {requestId}";
            return _dbClient.RunCmdAsync(query);
        }

        public Task ApproveAsync(int requestId)
        {
            var query = $"UPDATE {DbClient.FriendsTable} SET `Status` = {FriendRequestStatus.Approved:D} WHERE Id = {requestId}";
            return _dbClient.RunCmdAsync(query);
        }
    }
}