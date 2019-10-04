using System;
using ItemObserveApp.Models.Domain;

namespace ItemObserveApp.Models.Validator
{
    public class ItemValidator : IValidate<Item>
    {
        public string Validate(Item target)
        {
            if (target == null)
            {
                return "item is null";
            }
            if (target.ThretholdPrice < 1)
            {
                return "ThretholdPrice is less than 1";
            }
            if (string.IsNullOrWhiteSpace(target.ProductName))
            {
                return "ProductName is empty";
            }
            if (string.IsNullOrWhiteSpace(target.GroupID))
            {
                return "GroupID is empty";
            }
            if (string.IsNullOrWhiteSpace(target.ProductID))
            {
                return "ProductID is empty";
            }
            if (string.IsNullOrWhiteSpace(target.UserID))
            {
                return "UserID is empty";
            }
            return string.Empty;
        }
    }
}
