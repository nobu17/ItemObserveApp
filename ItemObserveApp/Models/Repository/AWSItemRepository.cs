using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItemObserveApp.Common;
using ItemObserveApp.Models.Domain;
using Newtonsoft.Json;

namespace ItemObserveApp.Models.Repository
{
    public class AWSItemRepository : IItemRepository
    {
        private readonly IUserRepository _userRepository;

        // put時の比較用に保持する
        private static IEnumerable<Item> _lastGetItems;

        public AWSItemRepository(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            if (_userRepository == null)
            {
                throw new ArgumentException("userRepository is null");
            }
        }

        public async Task DeleteItemAsync(Item target)
        {
            // guidが一致するものがあれば間引いて更新
            var itemList = _lastGetItems.Where(x => x.UniqueID != target.UniqueID);
            if (itemList.Count() == _lastGetItems.Count())
            {
                throw new ArgumentException("no item detected for delete");
            }

            var usr = await _userRepository.GetUserSettingAsync();
            // delete interfaceは無いので間引いてdelete
            var url = Util.Enviroment.APIURLBase + "/item";
            var clinet = new APIClinet<PutItemsResponce>();
            clinet.URL = url;
            clinet.ApiToken = usr.Token;
            var reqItem = new PutItemsParam();

            var putLists = new PutLists();
            putLists.UserID = target.UserID;
            putLists.GroupID = target.GroupID;
            putLists.ItemList = itemList.Select(x => new ItemInfo() { ThretholdPrice = x.ThretholdPrice, ItemName = x.ProductName, ProductID = x.ProductID, StoreType = StoreTypeUtil.GetStoreTypeFromEnum(x.StoreType) }).ToList();

            reqItem.PutLists = putLists;
            var res = await clinet.PostAsync(reqItem);
            if (res.Result != null)
            {
                return;
            }
            throw new Exception("API call failed, code:" + res.StatusCode);
        }

        public async Task<IEnumerable<Item>> GetItemListAsync(string userID, string groupID)
        {
            var usr = await _userRepository.GetUserSettingAsync();

            var url = Util.Enviroment.APIURLBase + "/item";
            var clinet = new APIClinet<GetItemResponce>();
            clinet.URL = url;
            clinet.ApiToken = usr.Token;
            var reqItem = new GetItemParam(userID, groupID);
            var res = await clinet.PostAsync(reqItem);
            if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _lastGetItems = new List<Item>();
                return new List<Item>();
            }
            else if (res.Result != null)
            {
                // to listしないと遅延評価でunique idがおかしくなる
                var items = res.Result.ItemList.Select(x => GetItemFromApiInfo(res.Result.UserID, res.Result.GroupID, x)).ToList();
                // put比較用に保持
                _lastGetItems = items.Select(x => x.CopyItem()).ToList();
                return items;
            }
            throw new Exception("API call failed, code:" + res.StatusCode);
        }

        public async Task PutItemAsync(Item target)
        {
            var usr = await _userRepository.GetUserSettingAsync();

            var url = Util.Enviroment.APIURLBase + "/item";
            var clinet = new APIClinet<PutItemsResponce>();
            clinet.URL = url;
            clinet.ApiToken = usr.Token;
            var reqItem = new PutItemsParam();
            reqItem.PutLists = GetPutList(target);
            var res = await clinet.PostAsync(reqItem);
            if (res.Result != null)
            {
                return;
            }
            throw new Exception("API call failed, code:" + res.StatusCode);
        }

        private PutLists GetPutList(Item target)
        {
            // guidが一致するものがあれば変更
            var sameItem = _lastGetItems.FirstOrDefault(x => x.UniqueID == target.UniqueID);
            if (sameItem != null)
            {
                sameItem.ProductID = target.ProductID;
                sameItem.ProductName = target.ProductName;
                sameItem.StoreType = target.StoreType;
                sameItem.ThretholdPrice = target.ThretholdPrice;
            }
            else
            {
                // なければ挿入
                _lastGetItems = _lastGetItems.Concat(new[] { target });
            }

            var putLists = new PutLists();
            putLists.UserID = target.UserID;
            putLists.GroupID = target.GroupID;
            putLists.ItemList = _lastGetItems.Select(x => new ItemInfo() { ThretholdPrice = x.ThretholdPrice, ItemName = x.ProductName, ProductID = x.ProductID, StoreType = StoreTypeUtil.GetStoreTypeFromEnum(x.StoreType) }).ToList();

            return putLists;
        }

        private Item GetItemFromApiInfo(string userID, string groupID, ItemInfo info)
        {
            var item = new Item();
            item.GroupID = groupID;
            item.UserID = userID;
            item.ProductID = info.ProductID;
            item.ProductName = info.ItemName;
            item.StoreType = StoreTypeUtil.GetStoreTypeFromString(info.StoreType);
            item.ThretholdPrice = info.ThretholdPrice;
            return item;
        }
    }

    internal class GetItemParam
    {
        public GetItemParam(string userID, string groupID)
        {
            Method = "get";
            ItemUserInfo = new ItemUserInfo() { UserID = userID, GroupID = groupID };
        }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("get_param")]
        public ItemUserInfo ItemUserInfo { get; set; }
    }

    internal class GetItemResponce
    {
        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("group_id")]
        public string GroupID { get; set; }

        [JsonProperty("item_masters")]
        public List<ItemInfo> ItemList { get; set; }
    }

    internal class PutItemsParam
    {
        public PutItemsParam()
        {
            Method = "put";
        }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("put_param")]
        public PutLists PutLists { get; set; }
    }

    internal class PutItemsResponce
    {
    }

    internal class ItemUserInfo
    {
        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("group_id")]
        public string GroupID { get; set; }
    }

    internal class PutLists
    {
        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("group_id")]
        public string GroupID { get; set; }

        [JsonProperty("item_masters")]
        public List<ItemInfo> ItemList { get; set; }
    }

    internal class ItemInfo
    {
        [JsonProperty("product_id")]
        public string ProductID { get; set; }

        [JsonProperty("store_type")]
        public string StoreType { get; set; }

        [JsonProperty("item_name")]
        public string ItemName { get; set; }

        [JsonProperty("threthold_price")]
        public int ThretholdPrice { get; set; }

        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("group_id")]
        public string GroupID { get; set; }
    }
}
