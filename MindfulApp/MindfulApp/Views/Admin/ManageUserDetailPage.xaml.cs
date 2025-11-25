using MindfulApp.Models;
using MindfulApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindfulApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageUserDetailPage : ContentPage
    {
        private readonly UserService _userService = new UserService();
        private readonly User _editingUser;

        public ManageUserDetailPage(User user)
        {
            InitializeComponent();
            _editingUser = user;

            if (_editingUser != null)
            {
                FullNameEntry.Text = _editingUser.FullName;
                EmailEntry.Text = _editingUser.Email;
                PhoneEntry.Text = _editingUser.Phone;
                LocationEntry.Text = _editingUser.Location;
                PasswordEntry.Text = _editingUser.Password;
                RolePicker.SelectedItem = _editingUser.Role;
            }
            NavigationPage.SetHasBackButton(this, false);

        }
        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminDashboardPage());
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string fullName = FullNameEntry.Text?.Trim();
            string email = EmailEntry.Text?.Trim();
            string phone = PhoneEntry.Text?.Trim();
            string location = LocationEntry.Text?.Trim();
            string password = PasswordEntry.Text;
            string role = RolePicker.SelectedItem?.ToString();

            // Required field validation
            if (string.IsNullOrEmpty(fullName) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(location) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(role))
            {
                await DisplayAlert("Error", "Please fill all required fields.", "OK");
                return;
            }

            // FullName length validation
            if (fullName.Length < 2)
            {
                await DisplayAlert("Error", "Full Name must be at least 2 characters.", "OK");
                return;
            }

            // Password length
            if (password.Length < 4)
            {
                await DisplayAlert("Error", "Password must be at least 4 characters.", "OK");
                return;
            }

            // Email validation
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                await DisplayAlert("Error", "Invalid email format.", "OK");
                return;
            }

            // Phone validation: only digits and 10 characters
            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\d{10}$"))
            {
                await DisplayAlert("Error", "Phone number must be 10 digits.", "OK");
                return;
            }

            // Role validation
            if (role != "User" && role != "Admin")
            {
                await DisplayAlert("Error", "Role must be either User or Admin.", "OK");
                return;
            }

            // Unique validation (FullName, Email, Phone)
            var existingUsers = await _userService.GetAllUsersAsync();

            // Check duplicate Full Name
            bool duplicateName = existingUsers.Any(u =>
                u.Id != _editingUser?.Id &&
                u.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase)
            );

            if (duplicateName)
            {
                await DisplayAlert("Error", "A user with this Full Name already exists.", "OK");
                return;
            }

            // Check duplicate Email
            bool duplicateEmail = existingUsers.Any(u =>
                u.Id != _editingUser?.Id &&
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
            );

            if (duplicateEmail)
            {
                await DisplayAlert("Error", "A user with this Email already exists.", "OK");
                return;
            }

            // Check duplicate Phone
            bool duplicatePhone = existingUsers.Any(u =>
                u.Id != _editingUser?.Id &&
                u.Phone == phone
            );

            if (duplicatePhone)
            {
                await DisplayAlert("Error", "A user with this Phone number already exists.", "OK");
                return;
            }

            // Build User object
            var user = new User
            {
                Id = _editingUser?.Id ?? 0,
                FullName = fullName,
                Email = email,
                Phone = phone,
                Location = location,
                Password = password,
                Role = role,
                CreatedDate = _editingUser?.CreatedDate ?? DateTime.UtcNow
            };

            // Save or update
            bool success = _editingUser == null
                ? await _userService.CreateUserAsync(user)
                : await _userService.UpdateUserAsync(user);

            if (success)
            {
                await DisplayAlert("Success", "User saved successfully.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Failed to save user.", "OK");
            }
        }
    }
}
