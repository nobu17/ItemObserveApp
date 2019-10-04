using ItemObserveApp.Models.Domain;
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

            // 起動直後にMainPageを表示する。
            NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<GroupEditPage>();
            containerRegistry.RegisterForNavigation<GroupListPage>();
            containerRegistry.RegisterForNavigation<ItemEditPage>();
            containerRegistry.RegisterForNavigation<ItemListPage>();
            containerRegistry.RegisterForNavigation<SettingPage>();

            containerRegistry.RegisterForNavigation<GroupEditPage, GroupEditPageViewModel>();
            containerRegistry.RegisterForNavigation<GroupListPage, GroupListPageViewModel>();
            containerRegistry.RegisterForNavigation<ItemEditPage, ItemEditViewModel>();
            containerRegistry.RegisterForNavigation<ItemListPage, ItemListViewModel>();
            containerRegistry.RegisterForNavigation<SettingPage, SettingViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

            containerRegistry.Register(typeof(IGroupRepository), typeof(MockGroupRepository));
            containerRegistry.Register(typeof(IItemRepository), typeof(MockItemRepository));
            containerRegistry.Register(typeof(IUserRepository), typeof(FileUserRepository));

            containerRegistry.Register(typeof(IValidate<ItemGroup>), typeof(ItemGroupValidator));
            containerRegistry.Register(typeof(IValidate<Item>), typeof(ItemValidator));
            containerRegistry.Register(typeof(IValidate<UserSetting>), typeof(UserSettingValidator));
        }
    }
}
