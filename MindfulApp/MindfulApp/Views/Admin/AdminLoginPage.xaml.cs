using MindfulApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindfulApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindfulApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminLoginPage : ContentPage
    {
        private readonly UserService _userService;

        public AdminLoginPage()
        {
            InitializeComponent();
            _userService = new UserService();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LandingPage());
        }

        private async void OnAdminSignInClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim().ToLower();
            string password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please enter both email and password.", "OK");
                return;
            }

            var users = await _userService.GetAllUsersAsync();
            var admin = users.Find(u => u.Email.ToLower() == email && u.Role == "Admin");

            if (admin == null)
            {
                await DisplayAlert("Error", "No admin found with this email.", "OK");
                return;
            }

            if (admin.Password != password)
            {
                await DisplayAlert("Error", "Incorrect password.", "OK");
                return;
            }

            SessionManager.LoggedInUser = admin;

            await DisplayAlert("Success", $"Welcome Admin {admin.FullName}!", "OK");
            await Navigation.PushAsync(new AdminDashboardPage());
        }
    }
}
