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
        public ItemListModel(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task InitModelAsync(string userID, string groupID)
        {
            var itemList = await _itemRepository.GetItemListAsync(userID, groupID);
            ItemList = new ObservableCollection<Item>(itemList);
        }

        public async Task DeleteItemAsync(Item target)
        {
            await _itemRepository.DeleteItemAsync(target);
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
