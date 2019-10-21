using System;
using System.Threading.Tasks;
using ItemObserveApp.Common;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;
using ItemObserveApp.Models.Validator;

namespace ItemObserveApp.Models
{
    public class LoginModel : BaseModel
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidate<UserSetting> _validator;
        public LoginModel(ILoginRepository loginRepository, IUserRepository userRepository, IValidate<UserSetting> validator)
        {
            _loginRepository = loginRepository;
            _userRepository = userRepository;
            _validator = validator;
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

        public override string Validate()
        {
            return _validator.Validate(_userSetting);
        }

        public async Task InitUserSettingAasync()
        {
            //ローカルに保存された情報を取得
            var user = await _userRepository.GetUserSettingAsync();
            if (user != null)
            {
                UserSetting = user;
            }
            else
            {
                UserSetting = new UserSetting();
            }
        }

        public async Task<Tuple<bool, string>> LoginAsync()
        {
            var result = await _loginRepository.LoginAsync(_userSetting.UserID, _userSetting.Password);
            if (result.IsSuccessed)
            {
                // store information
                UserSetting.Token = result.Token;
                await _userRepository.PutUserSettingAsync(UserSetting);
                return Tuple.Create(true, string.Empty);
            }
            else
            {
                UserSetting.Token = "";
                UserSetting.Password = "";
                RaisePropertyChanged("UserSetting");
                return Tuple.Create(false, result.Message);
            }
        }
    }
}
