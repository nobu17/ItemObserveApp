using System;
using Prism.Mvvm;
using Prism.Navigation;

namespace ItemObserveApp.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public void Destroy()
        {
        }

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
    }
}
