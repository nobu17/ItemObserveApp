using System;
namespace ItemObserveApp.Models.Domain
{
    public class Item
    {
        public Item()
        {
        }

        public string UserID { get; set; }

        public string GroupID { get; set; }

        public string ProductName { get; set; }

        public string ProductID { get; set; }

        public int ThretholdPrice { get; set; }
    }
}
