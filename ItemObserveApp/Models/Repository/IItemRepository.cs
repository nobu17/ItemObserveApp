using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemObserveApp.Models.Domain;

namespace ItemObserveApp.Models.Repository
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetItemListAsync(string userID, string groupID);
        Task PutItemAsync(Item target);
        Task DeleteItemAsync(Item target);
    }
}
