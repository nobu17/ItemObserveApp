using System;
using ItemObserveApp.Common;
using ItemObserveApp.Models.Repository;

namespace ItemObserveApp.Models.Domain
{
    public class ItemGroup
    {
        public ItemGroup()
        {
            // assign new uuid as new item
            GroupID = Guid.NewGuid().ToString("N");
        }

        public string UserID { get; set; }

        public string GroupID { get; set; }

        public string GroupName { get; set; }
    }
}
