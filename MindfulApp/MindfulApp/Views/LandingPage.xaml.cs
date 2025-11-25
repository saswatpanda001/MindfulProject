using MindfulApp.Views.Admin;
using MindfulApp.Views.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindfulApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void AdminButton_Clicked(object sender, EventArgs e)
        {
            // Navigate to Admin page
            await Navigation.PushAsync(new AdminLoginPage());
        }

        private async void UserButton_Clicked(object sender, EventArgs e)
        {
            // Navigate to User page
            await Navigation.PushAsync(new UserLoginPage());
        }
    }
}