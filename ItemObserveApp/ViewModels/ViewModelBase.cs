using System;
using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace ItemObserveApp.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible, IApplicationLifecycleAware
    {
        protected INavigationService NavigationService { get; private set; }
        protected IPageDialogService PageDialogService { get; private set; }

        public ViewModelBase(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            PageDialogService = pageDialogService;
        }

        #region dialog

        protected async Task ShowOKDialog(string title, string message)
        {
            await PageDialogService.DisplayAlertAsync(title, message, "OK");
        }

        protected Task<bool> ShowYesNoDialog(string title, string message)
        {
            return PageDialogService.DisplayAlertAsync(title, message, "はい", "いいえ");
        }

        protected Task<bool> ShowOKCancelDialog(string title, string message)
        {
            return PageDialogService.DisplayAlertAsync(title, message, "OK", "Cancel");
        }

        #endregion

        #region navigation

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public async void NavigateAsync(string page, NavigationParameters navigationParameters)
        {
            await NavigationService.NavigateAsync(page, navigationParameters);
        }

        public async void NavigateGoBackAsync()
        {
            await NavigationService.GoBackAsync();
        }

        public async void NavigateGoBackAsync(NavigationParameters navigationParameters)
        {
            await NavigationService.GoBackAsync(navigationParameters);
        }

        #endregion

        #region lifecycle

        public void Destroy()
        {
        }

        public virtual void OnResume()
        {
        }

        public virtual void OnSleep()
        {
            // ログインを自動実施

            // 失敗したらログイン画面
        }

        #endregion

        #region prop

        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
            }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                SetProperty(ref _errorMessage, value);
                RaisePropertyChanged("HasError");
            }
        }

        public bool HasError
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_errorMessage))
                {
                    return true;
                }
                return false;
            }
        }
        #endregion
    }
}
