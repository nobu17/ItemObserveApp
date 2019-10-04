using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItemObserveApp.Models.Domain;

namespace ItemObserveApp.Models.Repository
{
    public class MockItemRepository : IItemRepository
    {
        private IEnumerable<Item> _items;
        public MockItemRepository()
        {
            _items = Enumerable.Range(0, 5).Select(x => new Item() { ProductID = x.ToString(), ProductName = "prdouct" + x.ToString(), ThretholdPrice = x * 100 });
        }

        public async Task DeleteItemAsync(Item target)
        {
            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                _items = _items.Where(x => x.ProductID != target.ProductID);
            });
        }

        public async Task<IEnumerable<Item>> GetItemListAsync(string userID, string groupID)
        {
            return await Task<IEnumerable<Item>>.Run(() =>
            {
                Task.Delay(1000).Wait();
                foreach (var item in _items)
                {
                    item.UserID = userID;
                    item.GroupID = groupID;
                }
                return _items;
            });
        }

        public async Task PutItemAsync(Item target)
        {
            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                foreach (var item in _items)
                {
                    if (item.GroupID == target.GroupID && item.ProductID == target.ProductID)
                    {
                        item.ProductName = target.ProductName;
                        item.ThretholdPrice = target.ThretholdPrice;
                        return;
                    }
                    _items = _items.Append(target).OrderBy(x => x.ProductID);
                }
            });
        }
    }
}
