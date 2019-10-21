using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemObserveApp.Common;
using ItemObserveApp.Models.Domain;
using Newtonsoft.Json;

namespace ItemObserveApp.Models.Repository
{
    public class AWSGroupRepository : IGroupRepository
    {
        private readonly IUserRepository _userRepository;
        public AWSGroupRepository(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            if (_userRepository == null)
            {
                throw new ArgumentException("userRepository is null");
            }
        }

        public async Task DeleteGroupAsync(string userID, string groupID)
        {
            var usr = await _userRepository.GetUserSettingAsync();

            var url = Util.Enviroment.APIURLBase + "/user";
            var clinet = new APIClinet<DeleteItemResponce>();
            clinet.URL = url;
            clinet.ApiToken = usr.Token;
            var reqItem = new DeleteItemRequest(userID, groupID);
            var res = await clinet.PostAsync(reqItem);
            if (res.Result != null)
            {
                if (res.Result.FailedGroupIDList.Any())
                {
                    throw new Exception("Failed Update:" + res.Result.FailedGroupIDList[0]);
                }
                return;
            }
            throw new Exception("APi call failed, code:" + res.StatusCode);
        }

        public async Task<IEnumerable<ItemGroup>> GetItemGroupListAsync(string userID, string password)
        {
            var usr = await _userRepository.GetUserSettingAsync();

            var url = Util.Enviroment.APIURLBase + "/user";
            var clinet = new APIClinet<GetItemGroupResponce>();
            clinet.URL = url;
            clinet.ApiToken = usr.Token;
            var reqItem = new GetItemRequest(userID, password);
            var res = await clinet.PostAsync(reqItem);
            if (res.Result != null)
            {
                return res.Result.GroupList.Select(x => new ItemGroup() { UserID = userID, GroupID = x.GroupID, GroupName = x.GroupName });
            }
            throw new Exception("APi call failed, code:" + res.StatusCode);
        }

        public async Task PutGroupAsync(ItemGroup group)
        {
            var usr = await _userRepository.GetUserSettingAsync();

            var url = Util.Enviroment.APIURLBase + "/user";
            var clinet = new APIClinet<PutItemResponce>();
            clinet.URL = url;
            clinet.ApiToken = usr.Token;
            var reqItem = new PutItemRequest(group.UserID, group.GroupID, group.GroupName);
            var res = await clinet.PostAsync(reqItem);
            if (res.Result != null)
            {
                if (res.Result.FailedGroupIDList.Any())
                {
                    throw new Exception("Failed Update:" + res.Result.FailedGroupIDList[0]);
                }
                return;
            }
            throw new Exception("APi call failed, code:" + res.StatusCode);
        }
    }

    internal class GetItemRequest
    {
        public GetItemRequest(string userID, string password)
        {
            Method = "get";
            UserInfo = new UserInfo() { UserID = userID, Password = password };
        }
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("get_param")]
        public UserInfo UserInfo { get; set; }
    }

    internal class GetItemGroupResponce
    {
        [JsonProperty("item_group_list")]
        public List<GroupInfo> GroupList { get; set; }
    }

    internal class PutItemRequest
    {
        public PutItemRequest(string userID, string groupID, string groupName)
        {
            Method = "put_group";
            PutParam = new PutParam() { UserID = userID, GroupList = new List<GroupInfo>() { new GroupInfo() { GroupID = groupID, GroupName = groupName } } };
        }
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("put_group_param")]
        public PutParam PutParam { get; set; }
    }

    internal class PutParam
    {
        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("group_list")]
        public List<GroupInfo> GroupList { get; set; }
    }

    internal class PutItemResponce
    {
        [JsonProperty("success_group_id_list")]
        public List<string> SuccessGroupIDList { get; set; }

        [JsonProperty("failed_group_id_list")]
        public List<string> FailedGroupIDList { get; set; }
    }

    internal class DeleteItemRequest
    {
        public DeleteItemRequest(string userID, string groupID)
        {
            Method = "delete_group";
            DeleteParam = new DeleteParam() { UserID = userID, GroupIDList = new List<string>() { groupID } };
        }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("delete_group_param")]
        public DeleteParam DeleteParam { get; set; }
    }

    internal class DeleteParam
    {
        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("group_id_list")]
        public List<string> GroupIDList { get; set; }
    }

    internal class DeleteItemResponce
    {
        [JsonProperty("success_group_id_list")]
        public List<string> SuccessGroupIDList { get; set; }

        [JsonProperty("failed_group_id_list")]
        public List<string> FailedGroupIDList { get; set; }
    }

    internal class UserInfo
    {
        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
    internal class GroupInfo
    {
        [JsonProperty("group_id")]
        public string GroupID { get; set; }
        [JsonProperty("group_name")]
        public string GroupName { get; set; }
    }
}
