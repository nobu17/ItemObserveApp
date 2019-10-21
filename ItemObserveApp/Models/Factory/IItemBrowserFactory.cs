using System;
using ItemObserveApp.Models.Repository;

namespace ItemObserveApp.Models.Factory
{
    public interface IItemBrowserFactory
    {
        IItemBrowserRepository GetRepository(string url);
    }
}
