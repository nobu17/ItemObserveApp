using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ItemObserveApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ItemObserveApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemWebBrowserPage : ContentPage
    {
        public ItemWebBrowserPage()
        {
            InitializeComponent();
        }

        private void GoBack<T>(T sender)
        {
            webView.GoBack();
        }

        private void GoFoward<T>(T sender)
        {
            webView.GoForward();
        }

        void Handle_Disappearing(object sender, System.EventArgs e)
        {
            MessagingCenter.Unsubscribe<ItemWebBrowserPageViewModel>(this, "GoBack");
            MessagingCenter.Unsubscribe<ItemWebBrowserPageViewModel>(this, "GoForward");
        }

        void Handle_Appearing(object sender, System.EventArgs e)
        {
            MessagingCenter.Subscribe<ItemWebBrowserPageViewModel>(this, "GoBack", GoBack);
            MessagingCenter.Subscribe<ItemWebBrowserPageViewModel>(this, "GoForward", GoFoward);
        }
    }
}
