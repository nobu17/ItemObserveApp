using System;
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
        private UserSetting _userSetting;
        private string _groupID;
        public ItemListModel(IUserRepository userRepository, IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
            _userRepository = userRepository;
        }

        public async Task InitModelAsync(string groupID)
        {
            _userSetting = await _userRepository.GetUserSettingAsync();
            _groupID = groupID;
            var itemList = await _itemRepository.GetItemListAsync(_userSetting.UserID, groupID);
            ItemList = new ObservableCollection<Item>(itemList);
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

        private ObservableCollection<Item> _itemList;
        public ObservableCollection<Item> ItemList
        {
            get { return _itemList; }
            set
            {
                SetProperty(ref _itemList, value);
            }
        }


    }
}
