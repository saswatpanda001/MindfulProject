using MindfulApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindfulApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindfulApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageMeditationPage : ContentPage
    {
        private readonly MeditationService _service = new MeditationService();
        private List<MeditationSessionSelectable> _sessions = new List<MeditationSessionSelectable>();

        public ManageMeditationPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadSessionsAsync();
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminDashboardPage());
        }

        private readonly UserService _userService = new UserService();

        private async Task LoadSessionsAsync()
        {
            var sessions = await _service.GetAllSessionsAsync();
            var users = await _userService.GetAllUsersAsync();

            // Build the session list with username
            _sessions = sessions.Select(s =>
            {
                var user = users.FirstOrDefault(u => u.Id == s.UserId);
                return new MeditationSessionSelectable
                {
                    Id = s.Id,
                    Date = s.Date,
                    DurationMinutes = s.DurationMinutes,
                    UserId = s.UserId,
                    UserName = user != null ? $"{user.Id}: {user.FullName}" : "Unknown User",
                    IsSelected = false
                };
            }).ToList();

            SessionsListView.ItemsSource = _sessions;
            SelectAllCheckBox.IsChecked = false;
        }

        private void OnSelectAllChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value;
            foreach (var s in _sessions)
            {
                s.IsSelected = isChecked;
            }
            SessionsListView.ItemsSource = null;
            SessionsListView.ItemsSource = _sessions;
        }

        private async void OnDeleteSelectedClicked(object sender, EventArgs e)
        {
            var selectedSessions = _sessions.Where(s => s.IsSelected).ToList();
            if (!selectedSessions.Any())
            {
                await DisplayAlert("Alert", "No sessions selected.", "OK");
                return;
            }

            bool confirm = await DisplayAlert("Confirm", $"Are you sure you want to delete {selectedSessions.Count} session(s)?", "Yes", "No");
            if (!confirm) return;

            bool allDeleted = true;
            foreach (var session in selectedSessions)
            {
                bool success = await _service.DeleteSessionAsync(session.Id);
                if (!success) allDeleted = false;
            }

            if (allDeleted)
                await DisplayAlert("Success", "Selected sessions deleted.", "OK");
            else
                await DisplayAlert("Error", "Some sessions could not be deleted.", "OK");

            await LoadSessionsAsync();
        }
    }
}
