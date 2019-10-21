using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ItemObserveApp.Views
{
    public partial class GroupListPage : ContentPage
    {
        public GroupListPage()
        {
            InitializeComponent();
            //NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }
    }
}
