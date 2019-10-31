using System;
namespace ItemObserveApp.Models.Domain
{
    public class ItemPriceLog
    {
        public ItemPriceLog()
        {
        }

        public StoreType StoreType { get; set; }

        public string ProductID { get; set; }

        public int Price { get; set; }

        public string LastModified { get; set; }
    }
}
