using MindfulApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindfulApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindfulApp.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserLoginPage : ContentPage
    {
        private readonly UserService _userService;

        public UserLoginPage()
        {
            InitializeComponent();
            _userService = new UserService();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LandingPage());
        }

        private async void OnUserSignInClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim();
            string password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please enter both email and password.", "OK");
                return;
            }

            // Fetch user by email
            var users = await _userService.GetAllUsersAsync();
            var user = users.Find(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.Role == "User");

            if (user == null)
            {
                await DisplayAlert("Error", "No user found with this email.", "OK");
                return;
            }

            if (user.Password != password)
            {
                await DisplayAlert("Error", "Incorrect password.", "OK");
                return;
            }
            SessionManager.LoggedInUser = user;

            // Login success
            await DisplayAlert("Success", $"Welcome {user.FullName}!", "OK");
            await Navigation.PushAsync(new UserDashboardPage());
            // Navigate to user dashboard or main page
        }

        // Called when the user taps "Sign Up"
        private async void OnSignUpTapped(object sender, EventArgs e)
        {
            // Navigate to the SignUpPage
            await Navigation.PushAsync(new SignupPage());
        }

        // Called when the user taps "Forgot Password?"
        private async void OnForgotPasswordTapped(object sender, EventArgs e)
        {
            // Navigate to a ForgotPasswordPage
            await Navigation.PushAsync(new ForgotPasswordPage());

        }

    }
}