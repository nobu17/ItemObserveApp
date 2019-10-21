﻿using System;
using ItemObserveApp.Common;
using ItemObserveApp.Models.Repository;

namespace ItemObserveApp.Models.Factory
{
    public class ItemBrowserFactory : IItemBrowserFactory
    {
        public ItemBrowserFactory()
        {
        }

        public IItemBrowserRepository GetRepository(string url)
        {
            if (url.StartsWith(Urls.AmazonURL, StringComparison.Ordinal))
            {
                return new ItemAmazonBrowserRepository();
            }
            return null;
        }
    }
}
