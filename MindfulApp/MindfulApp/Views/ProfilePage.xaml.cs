using MindfulApp.Models;
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
    public partial class ProfilePage : ContentPage
    {
        private readonly UserService _userService;
        private readonly User _currentUser;

        public ProfilePage(User loggedInUser)
        {
            InitializeComponent();
            _userService = new UserService();
            _currentUser = loggedInUser;

            // Fill entries with current user's data
            FullNameEntry.Text = _currentUser.FullName;
            EmailEntry.Text = _currentUser.Email;
            PhoneEntry.Text = _currentUser.Phone;
            LocationEntry.Text = _currentUser.Location;
            NavigationPage.SetHasBackButton(this, false);
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

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string fullName = FullNameEntry.Text?.Trim();
            string email = EmailEntry.Text?.Trim();
            string phone = PhoneEntry.Text?.Trim();
            string location = LocationEntry.Text?.Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone))
            {
                await DisplayAlert("Error", "Full Name, Email, and Phone are required.", "OK");
                return;
            }

            // Check uniqueness
            var users = await _userService.GetAllUsersAsync();

            if (users.Any(u => u.Id != _currentUser.Id && u.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase)))
            {
                await DisplayAlert("Error", "Full Name is already taken by another user.", "OK");
                return;
            }

            if (users.Any(u => u.Id != _currentUser.Id && u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                await DisplayAlert("Error", "Email is already taken by another user.", "OK");
                return;
            }

            if (users.Any(u => u.Id != _currentUser.Id && u.Phone.Equals(phone)))
            {
                await DisplayAlert("Error", "Phone is already taken by another user.", "OK");
                return;
            }

            // Update current user
            _currentUser.FullName = fullName;
            _currentUser.Email = email;
            _currentUser.Phone = phone;
            _currentUser.Location = location;

            bool success = await _userService.UpdateUserAsync(_currentUser);

            if (success)
            {
                await DisplayAlert("Success", "Profile updated successfully!", "OK");
                if (SessionManager.LoggedInUser.Role == "Admin")
                {
                    await Navigation.PushAsync(new AdminDashboardPage());

                }
                else {
                    await Navigation.PushAsync(new UserDashboardPage());
                }
                
            }
            else
            {
                await DisplayAlert("Error", "Failed to update profile. Try again.", "OK");
            }
        }
    }
}