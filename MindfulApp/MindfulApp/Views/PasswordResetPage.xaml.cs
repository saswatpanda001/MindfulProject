using MindfulApp.Services;
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
    public partial class PasswordResetPage : ContentPage
    {
        private readonly UserService _userService;

        public PasswordResetPage()
        {
            InitializeComponent();
            _userService = new UserService();
            NavigationPage.SetHasBackButton(this, false);

            // Set logged-in user email
            if (SessionManager.LoggedInUser != null)
            {
                EmailEntry.Text = SessionManager.LoggedInUser.Email;
            }
            else {
                EmailEntry.Text = null;
            }
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            if (SessionManager.LoggedInUser.Role == "Admin")
            {
                await Navigation.PushAsync(new AdminDashboardPage());

            }
            else
            {
                await Navigation.PushAsync(new UserDashboardPage());
            }

        }

        private async void OnResetPasswordClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim();
            string currentPassword = CurrentPasswordEntry.Text;
            string newPassword = NewPasswordEntry.Text;
            string confirmNewPassword = ConfirmNewPasswordEntry.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(currentPassword) ||
                string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmNewPassword))
            {
                await DisplayAlert("Error", "Please fill all fields.", "OK");
                return;
            }

            if (newPassword != confirmNewPassword)
            {
                await DisplayAlert("Error", "New passwords do not match.", "OK");
                return;
            }

            // Fetch all users and find by email
            var users = await _userService.GetAllUsersAsync();
            var user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                await DisplayAlert("Error", "No user found with this email.", "OK");
                return;
            }

            if (user.Password != currentPassword)
            {
                await DisplayAlert("Error", "Current password is incorrect.", "OK");
                return;
            }

            // Update password
            user.Password = newPassword;
            bool success = await _userService.UpdateUserAsync(user);

            if (success)
            {
                await DisplayAlert("Success", "Password updated successfully!", "OK");
                if (SessionManager.LoggedInUser.Role == "Admin")
                {
                    await Navigation.PushAsync(new AdminDashboardPage());

                }
                else
                {
                    await Navigation.PushAsync(new UserDashboardPage());
                }
            }
            else
            {
                await DisplayAlert("Error", "Failed to update password. Try again.", "OK");
            }
        }
    }
}
