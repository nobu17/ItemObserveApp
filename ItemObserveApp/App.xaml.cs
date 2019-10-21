using ItemObserveApp.Common;
using ItemObserveApp.Models.Domain;
using ItemObserveApp.Models.Factory;
using ItemObserveApp.Models.Repository;
using ItemObserveApp.Models.Validator;
using ItemObserveApp.ViewModels;
using ItemObserveApp.Views;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using Xamarin.Forms;

namespace ItemObserveApp
{
    public partial class App : PrismApplication
    {
        public App()
        {
            InitializeComponent();
        }

        public App(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {

        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            // StartupPage
            var param = new NavigationParameters();
            param.Add("AutoLogin", "true");
            NavigationService.NavigateAsync(Route.LoginInitPage, param);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<GroupEditPage>();
            containerRegistry.RegisterForNavigation<GroupListPage>();
            containerRegistry.RegisterForNavigation<ItemEditPage>();
            containerRegistry.RegisterForNavigation<ItemListPage>();
            containerRegistry.RegisterForNavigation<ItemWebBrowserPage>();
            containerRegistry.RegisterForNavigation<MainPage>();

            containerRegistry.RegisterForNavigation<GroupEditPage, GroupEditPageViewModel>();
            containerRegistry.RegisterForNavigation<GroupListPage, GroupListPageViewModel>();
            containerRegistry.RegisterForNavigation<ItemEditPage, ItemEditViewModel>();
            containerRegistry.RegisterForNavigation<ItemListPage, ItemListViewModel>();
            containerRegistry.RegisterForNavigation<LoginInitPage, LoginInitPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginSettingPage, LoginSettingPageViewModel>();
            containerRegistry.RegisterForNavigation<ItemWebBrowserPage, ItemWebBrowserPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

            containerRegistry.Register(typeof(IGroupRepository), typeof(AWSGroupRepository));
            containerRegistry.Register(typeof(IItemRepository), typeof(AWSItemRepository));
            containerRegistry.Register(typeof(IUserRepository), typeof(FileUserRepository));
            containerRegistry.Register(typeof(ILoginRepository), typeof(AWSLoginRepository));
            containerRegistry.Register(typeof(IItemBrowserFactory), typeof(ItemBrowserFactory));


            containerRegistry.Register(typeof(IValidate<ItemGroup>), typeof(ItemGroupValidator));
            containerRegistry.Register(typeof(IValidate<Item>), typeof(ItemValidator));
            containerRegistry.Register(typeof(IValidate<UserSetting>), typeof(UserSettingValidator));
        }
    }
}
