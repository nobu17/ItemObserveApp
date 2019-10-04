using System;
using ItemObserveApp.Models.Domain;

namespace ItemObserveApp.Models.Validator
{
    public class ItemGroupValidator : IValidate<ItemGroup>
    {
        public string Validate(ItemGroup target)
        {
            if (target == null)
            {
                return "item is null";
            }
            if (string.IsNullOrWhiteSpace(target.GroupID))
            {
                return "GroupID is empty";
            }
            if (string.IsNullOrWhiteSpace(target.GroupName))
            {
                return "GroupName is empty";
            }
            if (string.IsNullOrWhiteSpace(target.UserID))
            {
                return "UserID is empty";
            }
            return string.Empty;
        }
    }
}
