using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemObserveApp.Models;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;
using ItemObserveApp.Models.Validator;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ItemObserveApp.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public LoginPageViewModel(ILoginRepository loginRepository, IUserRepository userRepository, IValidate<UserSetting> validator, INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            Model = new LoginModel(loginRepository, userRepository, validator);
            InitAsync();
        }

        private LoginModel _model;
        public LoginModel Model
        {
            get { return _model; }
            set
            {
                SetProperty(ref _model, value);
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new Command(() =>
                {
                    LoginAsync();
                });
            }
        }

        private async void InitAsync()
        {
            try
            {
                IsLoading = true;
                await _model.InitUserSettingAasync();
                // if id and pass existed try to login
                await LoginAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoginAsync()
        {
            ErrorMessage = _model.Validate();
            if (string.IsNullOrWhiteSpace(ErrorMessage))
            {
                try
                {
                    IsLoading = true;
                    var result = await _model.LogionAsync();
                    if (!result.Item1)
                    {
                        await ShowOKDialog("エラー", result.Item2);
                        return;
                    }
                    GoItemListPage();
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        private void GoItemListPage()
        {
            var param = new NavigationParameters();
            param.Add("UserSetting", _model.UserSetting);
            NavigateAsync("GroupListPage", param);
        }
    }
}
