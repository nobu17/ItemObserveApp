using ItemObserveApp.Models.Domain;

namespace ItemObserveApp.Models.Validator
{
    public class UserSettingValidator : IValidate<UserSetting>
    {
        public UserSettingValidator()
        {
        }

        public string Validate(UserSetting target)
        {
            if (target == null)
            {
                return "item is null";
            }
            if (string.IsNullOrWhiteSpace(target.UserID))
            {
                return "UserID is empty";
            }
            if (string.IsNullOrWhiteSpace(target.Password))
            {
                return "Password is empty";
            }
            return string.Empty;
        }
    }
}
