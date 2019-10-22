using System;
namespace ItemObserveApp.Models.Domain
{
    public class Item
    {
        public Item()
        {
            StoreType = StoreType.amazon;
            // assign new uuid as new item
            UniqueID = Guid.NewGuid().ToString("N");
        }

        public Item CopyItem()
        {
            var item = new Item();
            item.UniqueID = UniqueID;
            item.UserID = UserID;
            item.GroupID = GroupID;
            item.StoreType = StoreType;
            item.ProductName = ProductName;
            item.ProductID = ProductID;
            item.ThretholdPrice = ThretholdPrice;
            return item;
        }

        // まとめてputする際に識別用のID database上は保持しない
        public string UniqueID { get; private set; }

        public string UserID { get; set; }

        public string GroupID { get; set; }

        public StoreType StoreType { get; set; }

        public string ProductName { get; set; }

        public string ProductID { get; set; }

        public int ThretholdPrice { get; set; }

    }

    public enum StoreType
    {
        amazon,
        surugaya,
        bookoff
    }

    public class StoreTypeUtil
    {
        private const string AmazonStr = "amazon";
        private const string SurugayaStr = "surugaya";
        private const string BookOffStr = "bookoff";

        public static StoreType GetStoreTypeFromString(string storeType)
        {
            switch (storeType)
            {
                case AmazonStr:
                    return StoreType.amazon;
                case SurugayaStr:
                    return StoreType.surugaya;
                case BookOffStr:
                    return StoreType.bookoff;
                default:
                    throw new ArgumentException("not support string:" + storeType);
            }
        }

        public static string GetStoreTypeFromEnum(StoreType storeType)
        {
            switch (storeType)
            {
                case StoreType.amazon:
                    return AmazonStr;
                case StoreType.surugaya:
                    return SurugayaStr;
                case StoreType.bookoff:
                    return BookOffStr;
                default:
                    throw new ArgumentException("not support enum:" + storeType);
            }
        }
    }
}
