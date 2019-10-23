using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemObserveApp.Models;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;
using ItemObserveApp.Models.Validator;
using Prism;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ItemObserveApp.ViewModels
{
    public class LoginSettingPageViewModel : LoginPageBaseViewModel, IActiveAware
    {
        private bool _isInited = false;

        public LoginSettingPageViewModel(ILoginRepository loginRepository, IUserRepository userRepository, IValidate<UserSetting> validator, INavigationService navigationService, IPageDialogService pageDialogService)
            : base(loginRepository, userRepository, validator, navigationService, pageDialogService)
        {
            CanCancel = true;
        }


        public event EventHandler IsActiveChanged;
        // Tab selection changed
        private bool _isActive = false;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, RaiseIsActiveChanged); }
        }

        protected virtual async void RaiseIsActiveChanged()
        {
            if (_isActive && !_isInited)
            {
                await InitAsync();
                _isInited = true;
            }
        }

        protected async override void OnLoginSuccessed()
        {
            await ShowOKDialog("通知", "ログインに成功しました。");
        }

        protected override void OnLoginFailed()
        {
        }
    }
}
