using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemObserveApp.Models;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Factory;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ItemObserveApp.ViewModels
{
    public class ItemWebBrowserPageViewModel : ViewModelBase
    {
        private string _currentUrl;

        public ItemWebBrowserPageViewModel(IItemBrowserFactory itemBrowserFactory, INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
            Model = new ItemWebBrowserModel(itemBrowserFactory);
        }

        #region Command

        public Command Commit
        {
            get
            {
                return new Command(() =>
                {
                    LoadItemDataFromWebAsync();
                });
            }
        }

        public Command GoBack
        {
            get
            {
                return new Command(() =>
                {
                    MessagingCenter.Send(this, "GoBack");
                });
            }
        }

        public Command GoForward
        {
            get
            {
                return new Command(() =>
                {
                    MessagingCenter.Send(this, "GoForward");
                });
            }
        }

        public ICommand Navigated
        {
            get
            {
                return new Command<WebNavigatedEventArgs>((e) =>
                {
                    if (e.Result == WebNavigationResult.Success)
                    {
                        Model.ChangeCommitable(e.Url);
                        _currentUrl = e.Url;
                    }
                });
            }
        }

        public ICommand Navigating
        {
            get
            {
                return new Command(() =>
                {

                });
            }
        }


        #endregion

        #region prop

        public Func<string, Task<string>> EvaluateJavascript { get; set; }

        private ItemWebBrowserModel _model;
        public ItemWebBrowserModel Model
        {
            get { return _model; }
            set
            {
                SetProperty(ref _model, value);
            }
        }

        /// <summary>
        /// The source URL.
        /// </summary>
        private string _sourceUrl;
        public string SourceUrl
        {
            get { return _sourceUrl; }
            set
            {
                SetProperty(ref _sourceUrl, value);
            }
        }

        #endregion

        private async Task LoadItemDataFromWebAsync()
        {
            try
            {
                var html = await EvaluateJavascript("document.body.innerHTML");
                var title = await EvaluateJavascript("document.title");
                var item = Model.GetItem(_currentUrl, title, html);
                GoBackWithItem(item);
            }
            catch (Exception)
            {
                await ShowOKDialog("error", "取得に失敗しました。");
            }
        }

        private void GoBackWithItem(WebItemInfo item)
        {
            var naviParam = new NavigationParameters();
            naviParam.Add("WebItem", item);
            NavigateGoBackAsync(naviParam);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("URL"))
            {
                SourceUrl = parameters["URL"] as string;
                Model.InitFromUrl(SourceUrl);
            }
            base.OnNavigatedTo(parameters);
        }
    }
}
