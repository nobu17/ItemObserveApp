using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ItemObserveApp.Common;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;

namespace ItemObserveApp.Models
{
    public class ItemListModel : BaseModel
    {
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IItemPriceLogRepository _itemPriceLogRepository;
        private UserSetting _userSetting;
        private string _groupID;
        public ItemListModel(IUserRepository userRepository, IItemRepository itemRepository, IItemPriceLogRepository itemPriceLogRepository)
        {
            _userRepository = userRepository;
            if (_userRepository == null)
            {
                throw new ArgumentException("userRepository is null");
            }
            _itemRepository = itemRepository;
            if (_itemRepository == null)
            {
                throw new ArgumentException("itemRepository is null");
            }
            _itemPriceLogRepository = itemPriceLogRepository;
            if (_itemPriceLogRepository == null)
            {
                throw new ArgumentException("itemPriceLogRepository is null");
            }
        }

        public async Task InitModelAsync(string groupID)
        {
            _userSetting = await _userRepository.GetUserSettingAsync();
            _groupID = groupID;
            var itemList = await _itemRepository.GetItemListAsync(_userSetting.UserID, groupID);
            var items = new ObservableCollection<ItemAndPriceLog>(itemList.Select(x => new ItemAndPriceLog() { Item = x }));
            // combine pricelog
            var priceLogList = await _itemPriceLogRepository.GetItemPriceLogListAsync(_userSetting.UserID, groupID);

            foreach (var item in items)
            {
                var sameIDItemPriceLog = priceLogList.FirstOrDefault(x => x.ProductID == item.Item.ProductID && x.StoreType == item.Item.StoreType);
                if (sameIDItemPriceLog != null)
                {
                    item.PriceLog = sameIDItemPriceLog;
                }
                else
                {
                    item.PriceLog = new ItemPriceLog();
                }
            }
            ItemList = items;
        }

        public async Task DeleteItemAsync(Item target)
        {
            await _itemRepository.DeleteItemAsync(target);
        }

        public Item GetNewItem()
        {
            var item = new Item();
            item.UserID = _userSetting.UserID;
            item.GroupID = _groupID;
            item.ProductName = "";
            item.ThretholdPrice = 0;
            return item;
        }

        private ObservableCollection<ItemAndPriceLog> _itemList;
        public ObservableCollection<ItemAndPriceLog> ItemList
        {
            get { return _itemList; }
            set
            {
                SetProperty(ref _itemList, value);
            }
        }
    }

    public class ItemAndPriceLog
    {
        public Item Item { get; set; }

        public ItemPriceLog PriceLog { get; set; }
    }
}
