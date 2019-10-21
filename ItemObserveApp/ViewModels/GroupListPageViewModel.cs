using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemObserveApp.Common;
using ItemObserveApp.Models;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ItemObserveApp.ViewModels
{
    public class GroupListPageViewModel : ViewModelBase
    {
        public GroupListPageViewModel(IUserRepository userRepository, IGroupRepository groupRepository, INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
            _model = new GroupListModel(userRepository, groupRepository);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            LoadGroupAsync();
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

        public ICommand MakeNewCommand
        {
            get
            {
                return new Command(() =>
                {
                    var param = new NavigationParameters();
                    param.Add("ItemGroup", _model.GetNewItemGroup());
                    NavigateAsync(Route.GroupEditPage, param);
                });
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
                        NavigateAsync(Route.GroupEditPage, param);
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
                        param.Add("GroupID", item.GroupID);
                        NavigateAsync(Route.ItemListPage, param);
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
            }
            catch (UnAuthException)
            {
                // 認証エラーの場合はログインページへ
                NavigateAsync(Route.LoginInitPage, new NavigationParameters());
            }
            catch (Exception e)
            {
                await ShowOKDialog("エラー", "エラーが発生しました。" + e.ToString());
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}