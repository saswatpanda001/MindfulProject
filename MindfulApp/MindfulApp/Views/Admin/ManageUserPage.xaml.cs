using MindfulApp.Models;
using MindfulApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindfulApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageUserPage : ContentPage
    {
        private readonly UserService _userService = new UserService();

        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        public ManageUserPage()
        {
            InitializeComponent();
            BindingContext = this;
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadUsersAsync();
        }

        private async System.Threading.Tasks.Task LoadUsersAsync()
        {
            Users.Clear();
            var users = await _userService.GetAllUsersAsync();
            foreach (var user in users)
                Users.Add(user);
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminDashboardPage());
        }

        private async void OnAddNewUserClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageUserDetailPage(null));
        }

        private async void OnEditUserClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int userId)
            {
                var selectedUser = Users.FirstOrDefault(u => u.Id == userId);
                if (selectedUser != null)
                    await Navigation.PushAsync(new ManageUserDetailPage(selectedUser));
            }
        }

        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int userId)
            {
                bool confirm = await DisplayAlert("Confirm", "Are you sure you want to delete this user?", "Yes", "No");
                if (!confirm) return;

                bool isDeleted = await _userService.DeleteUserAsync(userId);
                if (isDeleted)
                {
                    await DisplayAlert("Success", "User deleted successfully.", "OK");
                    var userToRemove = Users.FirstOrDefault(u => u.Id == userId);
                    if (userToRemove != null) Users.Remove(userToRemove);
                }
                else
                {
                    await DisplayAlert("Error", "Failed to delete user.", "OK");
                }
            }
        }


    }
}