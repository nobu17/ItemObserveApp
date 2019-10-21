using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemObserveApp.Common;
using ItemObserveApp.Models;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Repository;
using ItemObserveApp.Models.Validator;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ItemObserveApp.ViewModels
{
    public class ItemEditViewModel : ViewModelBase
    {
        public ItemEditViewModel(IItemRepository itemRepository, IValidate<Item> validater, INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
            Model = new ItemEditModel(itemRepository, validater);
        }

        private ItemEditModel _model;
        public ItemEditModel Model
        {
            get { return _model; }
            set
            {
                SetProperty(ref _model, value);
            }
        }

        #region command

        public ICommand OpenAmazonCommand
        {
            get
            {
                return new Command(() =>
                {
                    var param = new NavigationParameters();
                    param.Add("URL", "https://www.amazon.co.jp");
                    NavigateAsync(Route.ItemWebBrowserPage, param);
                });
            }
        }

        public ICommand CommitCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CommitAsync();
                });
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                return new Command(() =>
                {
                    GoBack();
                });
            }
        }

        #endregion

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Item"))
            {
                _model.EditTarget = parameters["Item"] as Item;
            }
            else if (parameters.ContainsKey("WebItem"))
            {
                var item = parameters["WebItem"] as WebItemInfo;
                _model.UpdateEditTarget(item);
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
                    await _model.CommitAsync();
                    GoBack();
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

        private void GoBack()
        {
            NavigateGoBackAsync();
        }
    }

    public class ItemEditViewModelTransitParam
    {
        public Item EditItem { get; set; }
    }
}
