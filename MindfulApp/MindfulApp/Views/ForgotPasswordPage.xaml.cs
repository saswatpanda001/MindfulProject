using MindfulApp.Services;
using MindfulApp.Views.Admin;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindfulApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        private readonly UserService _userService;

        public ForgotPasswordPage()
        {
            InitializeComponent();
            _userService = new UserService();
            NavigationPage.SetHasBackButton(this, false);


        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new LandingPage());
            

        }


        private async void OnResetPasswordClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim();
            string newPassword = NewPasswordEntry.Text;
            string confirmNewPassword = ConfirmNewPasswordEntry.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmNewPassword))
            {
                await DisplayAlert("Error", "Please fill all fields.", "OK");
                return;
            }

            if (newPassword != confirmNewPassword)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            // Fetch users and find by email
            var users = await _userService.GetAllUsersAsync();
            var user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                await DisplayAlert("Error", "No user found with this email.", "OK");
                return;
            }

            // Update password
            user.Password = newPassword;
            bool success = await _userService.UpdateUserAsync(user);

            if (success)
            {
                await DisplayAlert("Success", "Password updated successfully!", "OK");
                await Navigation.PopAsync(); // Go back to login page
            }
            else
            {
                await DisplayAlert("Error", "Failed to update password. Try again.", "OK");
            }
        }
    }
}
