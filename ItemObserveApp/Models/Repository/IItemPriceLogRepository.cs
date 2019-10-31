using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemObserveApp.Models.Domain;

namespace ItemObserveApp.Models.Repository
{
    public interface IItemPriceLogRepository
    {
        Task<IEnumerable<ItemPriceLog>> GetItemPriceLogListAsync(string userID, string groupID);
    }
}
