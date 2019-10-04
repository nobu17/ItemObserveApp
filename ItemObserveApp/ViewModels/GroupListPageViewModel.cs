using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemObserveApp.Models;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;
using Prism.Navigation;
using Xamarin.Forms;

namespace ItemObserveApp.ViewModels
{
    public class GroupListPageViewModel : ViewModelBase
    {
        public GroupListPageViewModel(IGroupRepository groupRepository, INavigationService navigationService)
            : base(navigationService)
        {
            _model = new GroupListModel(groupRepository);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                _model.UserSetting = parameters["UserSetting"] as UserSetting;
                LoadGroupAsync();
            }
        }


        private GroupListModel _model;
        public GroupListModel Model
        {
            get { return _model; }
            set
            {
                SetProperty(ref _model, value);
            }
        }

        public ICommand EditCommand
        {
            get
            {
                return new Command<ItemGroup>((item) =>
                {
                    if (item != null)
                    {
                        var param = new NavigationParameters();
                        param.Add("ItemGroup", item);
                        NavigateAsync("GroupEditPage", param);
                    }
                });
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new Command<ItemGroup>((item) =>
                {
                    if (item != null)
                    {
                        DeleteGroupAsync(item);
                    }
                });
            }
        }

        public ICommand ReloadCommand { get; private set; }

        public ICommand ItemSelectedCommand
        {
            get
            {
                return new Command<ItemGroup>((item) =>
                {
                    if (item != null)
                    {
                        var param = new NavigationParameters();
                        param.Add("UserID", item.UserID);
                        param.Add("GroupID", item.GroupID);
                        NavigateAsync("ItemListPage", param);
                    }
                });
            }
        }

        private async Task DeleteGroupAsync(ItemGroup target)
        {
            try
            {
                IsLoading = true;
                await _model.DeleteGroupAsync(target);
                await _model.InitModelAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadGroupAsync()
        {
            try
            {
                IsLoading = true;
                await _model.InitModelAsync();
                // TODO エラーや認証失敗の場合は再度ログイン画面へ
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}