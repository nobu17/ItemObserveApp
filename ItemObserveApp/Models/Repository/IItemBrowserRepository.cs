using System;
using ItemObserveApp.Models.Domain;

namespace ItemObserveApp.Models.Repository
{
    public interface IItemBrowserRepository
    {
        bool IsComitable(string url);
        WebItemInfo GetItemInfo(string url, string title, string bodyhtml);
    }
}
