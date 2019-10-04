using System.Threading.Tasks;
using System.Windows.Input;
using ItemObserveApp.Models;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;
using Prism.Navigation;
using Xamarin.Forms;

namespace ItemObserveApp.ViewModels
{
    public class ItemListViewModel : ViewModelBase
    {
        public ItemListViewModel(IItemRepository itemRepository, INavigationService navigationService)
            : base(navigationService)
        {
            _model = new ItemListModel(itemRepository);
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
        public ICommand ReloadCommand { get; private set; }
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
                        NavigateAsync("ItemEditPage", param);
                    }
                });
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                var userID = parameters["UserID"] as string;
                var password = parameters["Password"] as string;
                InitModelAsync(userID, password);
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

        private async Task InitModelAsync(string userID, string groupID)
        {
            try
            {
                IsLoading = true;
                await _model.InitModelAsync(userID, groupID);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
