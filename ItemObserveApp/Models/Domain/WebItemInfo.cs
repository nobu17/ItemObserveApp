﻿using System;
namespace ItemObserveApp.Models.Domain
{
    public class WebItemInfo
    {
        public WebItemInfo()
        {
        }

        public StoreType StoreType { get; set; }

        public string ItemName { get; set; }

        public string ProductID { get; set; }

        public int Price { get; set; }
    }
}
