using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemObserveApp.Models.Domain;

namespace ItemObserveApp.Models.Repository
{
    public interface IGroupRepository
    {
        Task<IEnumerable<ItemGroup>> GetItemGroupListAsync(string userID, string password);
        Task DeleteGroupAsync(string userID, string groupID);
        Task PutGroupAsync(ItemGroup group);
    }
}
