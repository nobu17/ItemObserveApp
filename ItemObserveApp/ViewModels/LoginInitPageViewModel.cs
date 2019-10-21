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
    public class LoginInitPageViewModel : LoginPageBaseViewModel
    {
        public LoginInitPageViewModel(ILoginRepository loginRepository, IUserRepository userRepository, IValidate<UserSetting> validator, INavigationService navigationService, IPageDialogService pageDialogService)
            : base(loginRepository, userRepository, validator, navigationService, pageDialogService)
        {
            CanCancel = false;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await InitAsync();
            // 画面遷移の場合は自動ログイン
            if (parameters.ContainsKey("AutoLogin") && parameters["AutoLogin"].ToString().ToLower() == "true")
            {
                await LoginAsync();
            }
        }

        protected override void OnLoginSuccessed()
        {
            var param = new NavigationParameters();
            NavigateAsync("./MainPage", param);
        }

        protected override void OnLoginFailed()
        {
        }
    }
}
