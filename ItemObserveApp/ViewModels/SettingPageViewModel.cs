using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemObserveApp.Models;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;
using ItemObserveApp.Models.Validator;
using Prism.Navigation;
using Xamarin.Forms;

namespace ItemObserveApp.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        public SettingViewModel(IUserRepository userRepository, IValidate<UserSetting> validator, INavigationService navigationService)
            : base(navigationService)
        {
            _model = new SettingModel(userRepository, validator);
            InitAsync();
        }

        public ICommand CommitCommand
        {
            get
            {
                return new Command(() =>
                {
                    CommitAsync();
                });
            }
        }

        private async void InitAsync()
        {
            try
            {
                IsLoading = true;
                await _model.LoadSettingAsync();
                // modelが既に有効ならば一覧へ移動
                if (string.IsNullOrWhiteSpace(_model.Validate()))
                {
                    GoGroupListPage();
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task CommitAsync()
        {
            ErrorMessage = _model.Validate();
            if (ErrorMessage == string.Empty)
            {
                try
                {
                    IsLoading = true;
                    await _model.PutSettingAsync();
                    GoGroupListPage();
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        private void GoGroupListPage()
        {
            var param = new NavigationParameters();
            param.Add("UserSetting", _model.UserSetting);
            NavigateAsync("GroupListPage", param);
        }

        private SettingModel _model;
        public SettingModel Model
        {
            get { return _model; }
            set
            {
                SetProperty(ref _model, value);
            }
        }
    }
}
