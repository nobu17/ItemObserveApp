using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ItemObserveApp.Models.Domain;

namespace ItemObserveApp.Models.Repository
{
    public class MockGroupRepository : IGroupRepository
    {
        private IEnumerable<ItemGroup> _items = Enumerable.Range(0, 10).Select(x => new ItemGroup() { GroupID = x.ToString(), GroupName = "Group" + x.ToString() });
        public async Task DeleteGroupAsync(string userID, string groupID)
        {
            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                _items = _items.Where(x => x.GroupID != groupID);
            });
        }

        public async Task<IEnumerable<ItemGroup>> GetItemGroupListAsync(string userID, string password)
        {
            return await Task<IEnumerable<ItemGroup>>.Run(() =>
            {
                Task.Delay(1000).Wait();
                _items = _items.Select(x => new ItemGroup() { UserID = userID, GroupID = x.GroupID, GroupName = x.GroupName });
                return _items;
            });
        }

        public async Task PutGroupAsync(ItemGroup group)
        {
            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                foreach (var item in _items)
                {
                    if (item.GroupID == group.GroupID)
                    {
                        item.GroupName = group.GroupName;
                        return;
                    }
                }
                var temp = _items.Append(group).OrderBy(x => x.GroupID).ToList();
                _items = temp;
            });
        }
    }
}
