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

        public ICommand DeleteCommand
        {
            get
            {
                return new Command<Item>((item) =>
                {
                    if (item != null)
                    {
                        DeleteItemAsync(item);
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

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("GroupID"))
            {
                await InitModelAsync(parameters["GroupID"] as string);
            }
            else
            {
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
            finally
            {
                IsLoading = false;
            }
        }
    }
}
