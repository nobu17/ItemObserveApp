using System;
using System.Threading.Tasks;
using ItemObserveApp.Common;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;
using ItemObserveApp.Models.Validator;

namespace ItemObserveApp.Models
{
    public class SettingModel : BaseModel
    {
        private IUserRepository _userRepository;
        private IValidate<UserSetting> _validater;
        public SettingModel(IUserRepository userRepository, IValidate<UserSetting> validator)
        {
            _userRepository = userRepository;
            _validater = validator;
        }

        public async Task LoadSettingAsync()
        {
            UserSetting = await _userRepository.GetUserSettingAsync();
        }

        public async Task PutSettingAsync()
        {
            await _userRepository.PutUserSettingAsync(_userSetting);
        }

        public override string Validate()
        {
            return _validater.Validate(_userSetting);
        }

        private UserSetting _userSetting;
        public UserSetting UserSetting
        {
            get { return _userSetting; }
            set
            {
                SetProperty(ref _userSetting, value);
            }
        }
    }
}
