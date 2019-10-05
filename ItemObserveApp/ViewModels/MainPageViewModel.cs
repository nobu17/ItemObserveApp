using System;
using Prism.Navigation;
using Prism.Services;

namespace ItemObserveApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }
    }
}
