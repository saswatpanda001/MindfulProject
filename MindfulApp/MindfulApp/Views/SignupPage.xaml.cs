using MindfulApp.Models;
using MindfulApp.Services;
using MindfulApp.Views.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.Permissions;

namespace MindfulApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {
        private readonly UserService _userService;

        public SignupPage()
        {
            InitializeComponent();
            _userService = new UserService(); // Use the service we made earlier
            NavigationPage.SetHasBackButton(this, false);
        }


        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LandingPage());
        }


        private async void OnCreateAccountClicked(object sender, EventArgs e)
        {
            string fullName = FullNameEntry.Text?.Trim();
            string email = EmailEntry.Text?.Trim();
            string password = PasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;
            string phone = PhoneEntry.Text?.Trim();
            string location = LocationEntry.Text?.Trim();

            // Empty field validation
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(location) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                await DisplayAlert("Error", "Please fill all fields.", "OK");
                return;
            }

            // Email format validation
            if (!email.Contains("@") || !email.Contains("."))
            {
                await DisplayAlert("Error", "Please enter a valid email address.", "OK");
                return;
            }

            // Phone numeric check
            if (!phone.All(char.IsDigit) || phone.Length != 10)
            {
                await DisplayAlert("Error", "Phone number must be exactly 10 digits.", "OK");
                return;
            }

            // Confirm password
            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            // Terms check
            if (!TermsCheckBox.IsChecked)
            {
                await DisplayAlert("Error", "You must agree to the Terms and Privacy Policy.", "OK");
                return;
            }

            // ⛔ Fetch all users to check duplicates
            var existingUsers = await _userService.GetAllUsersAsync();

            // Check Full Name
            if (existingUsers.Any(u =>
                u.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase)))
            {
                await DisplayAlert("Error", "Full Name already exists.", "OK");
                return;
            }

            // Check Email
            if (existingUsers.Any(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                await DisplayAlert("Error", "Email already exists.", "OK");
                return;
            }

            // Check Phone
            if (existingUsers.Any(u => u.Phone == phone))
            {
                await DisplayAlert("Error", "Phone number already exists.", "OK");
                return;
            }

            // Create user object
            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                Phone = phone,
                Location = location,
                Password = password,
                Role = "User",
                CreatedDate = DateTime.Now
            };

            bool isCreated = await _userService.CreateUserAsync(newUser);

            if (isCreated)
            {
                await DisplayAlert("Success", "Account created successfully!", "OK");
                await Navigation.PushAsync(new UserLoginPage());
            }
            else
            {
                await DisplayAlert("Error", "Failed to create account. Try again.", "OK");
            }
        }
    }
}