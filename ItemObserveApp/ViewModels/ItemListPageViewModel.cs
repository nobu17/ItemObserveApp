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
    public class ItemListViewModel : ViewModelBase
    {
        private string _groupID;

        public ItemListViewModel(IUserRepository userRepository, IItemRepository itemRepository, INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
            _model = new ItemListModel(userRepository, itemRepository);
        }

        private ItemListModel _model;
        public ItemListModel Model
        {
            get { return _model; }
            set
            {
                SetProperty(ref _model, value);
            }
        }

        #region command

        public ICommand DeleteCommand
        {
            get
            {
                return new Command<Item>(async (item) =>
                {
                    if (item != null)
                    {
                        await DeleteItemAsync(item);
                        await InitModelAsync(_groupID);
                    }
                });
            }
        }

        public ICommand ItemSelectedCommand
        {
            get
            {
                return new Command<Item>((item) =>
                {
                    if (item != null)
                    {
                        var param = new NavigationParameters();
                        param.Add("Item", item);
                        NavigateAsync(Route.ItemEditPage, param);
                    }
                });
            }
        }

        public ICommand MakeNewCommand
        {
            get
            {
                return new Command(() =>
                {
                    var param = new NavigationParameters();
                    param.Add("Item", _model.GetNewItem());
                    NavigateAsync(Route.ItemEditPage, param);
                });
            }
        }

        #endregion

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("GroupID"))
            {
                _groupID = parameters["GroupID"] as string;
                await InitModelAsync(_groupID);
            }
            else if (parameters.ContainsKey("GoBack"))
            {
                //戻ってきた場合
                await InitModelAsync(_groupID);
            }
            else
            {
                _groupID = "";
                await ShowOKDialog("パラメータエラー", "GroupIDが指定されていません。");
                NavigateGoBackAsync();
            }
        }

        private async Task DeleteItemAsync(Item target)
        {
            try
            {
                IsLoading = true;
                await _model.DeleteItemAsync(target);
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

        private async Task InitModelAsync(string groupID)
        {
            try
            {
                IsLoading = true;
                await _model.InitModelAsync(groupID);
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
