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
    public abstract class LoginPageBaseViewModel : ViewModelBase
    {
        public LoginPageBaseViewModel(ILoginRepository loginRepository, IUserRepository userRepository, IValidate<UserSetting> validator, INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            Model = new LoginModel(loginRepository, userRepository, validator);
        }

        private bool _canCancel;
        public bool CanCancel
        {
            get { return _canCancel; }
            set
            {
                SetProperty(ref _canCancel, value);
            }
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


        public ICommand CancelCommand
        {
            get
            {
                return new Command(() =>
                {
                    GoBackPage();
                });
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

        protected async Task InitAsync()
        {
            try
            {
                // restore data from file 
                IsLoading = true;
                await _model.InitUserSettingAasync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected async Task LoginAsync()
        {
            ErrorMessage = _model.Validate();
            if (string.IsNullOrWhiteSpace(ErrorMessage))
            {
                try
                {
                    IsLoading = true;
                    var result = await _model.LoginAsync();
                    if (!result.Item1)
                    {
                        await ShowOKDialog("エラー", result.Item2);
                        OnLoginFailed();
                        return;
                    }
                    OnLoginSuccessed();
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        protected abstract void OnLoginSuccessed();
        protected abstract void OnLoginFailed();

        private void GoItemListPage()
        {
            var param = new NavigationParameters();
            param.Add("UserSetting", _model.UserSetting);
            NavigateAsync("./MainPage", param);
        }

        protected void GoBackPage()
        {
            NavigateGoBackAsync();
        }
    }
}
