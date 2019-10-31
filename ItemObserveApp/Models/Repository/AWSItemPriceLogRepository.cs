using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemObserveApp.Common;
using ItemObserveApp.Models.Domain;
using Newtonsoft.Json;

namespace ItemObserveApp.Models.Repository
{
    public class AWSItemPriceLogRepository : IItemPriceLogRepository
    {
        private readonly IUserRepository _userRepository;

        public AWSItemPriceLogRepository(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            if (_userRepository == null)
            {
                throw new ArithmeticException("userrepository is null");
            }
        }

        public async Task<IEnumerable<ItemPriceLog>> GetItemPriceLogListAsync(string userID, string groupID)
        {
            var usr = await _userRepository.GetUserSettingAsync();

            var url = Util.Enviroment.APIURLBase + "/pricelog";
            var clinet = new APIClinet<GetItemPriceLogResponce>();
            clinet.URL = url;
            clinet.ApiToken = usr.Token;
            var reqItem = new GetItemPriceLogParam(userID, groupID);
            var res = await clinet.PostAsync(reqItem);
            if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<ItemPriceLog>();
            }
            else if (res.Result != null)
            {
                var list = res.Result.PriceLogInfoList.Select(x => GetItemPriceLog(x));
                return list;
            }
            throw new Exception("API call failed, code:" + res.StatusCode);
        }

        private ItemPriceLog GetItemPriceLog(PriceLogInfo info)
        {
            var item = new ItemPriceLog();
            item.ProductID = info.ProductID;
            item.StoreType = StoreTypeUtil.GetStoreTypeFromString(info.StoreType);
            item.Price = info.Price;
            item.LastModified = info.LastModified;
            return item;
        }
    }

    internal class GetItemPriceLogParam
    {
        public GetItemPriceLogParam(string userID, string groupID)
        {
            Method = "get";
            PriceLogGetInfo = new PriceLogGetInfo() { UserID = userID, GroupID = groupID };
        }

        [JsonProperty("method")]
        public string Method { get; private set; }

        [JsonProperty("get_param")]
        public PriceLogGetInfo PriceLogGetInfo { get; set; }
    }

    internal class GetItemPriceLogResponce
    {
        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("group_id")]
        public string GroupID { get; set; }

        [JsonProperty("price_log_list")]
        public List<PriceLogInfo> PriceLogInfoList { get; set; }
    }

    internal class PriceLogGetInfo
    {
        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("group_id")]
        public string GroupID { get; set; }
    }

    internal class PriceLogInfo
    {
        [JsonProperty("item_id")]
        public string ProductID { get; set; }

        [JsonProperty("store_type")]
        public string StoreType { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("last_modified_datetime")]
        public string LastModified { get; set; }
    }
}
