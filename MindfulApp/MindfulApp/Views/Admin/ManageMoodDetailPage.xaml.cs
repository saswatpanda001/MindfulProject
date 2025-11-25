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
    public partial class ManageMoodDetailPage : ContentPage
    {
        private readonly MoodService _moodService = new MoodService();
        private readonly UserService _userService = new UserService();
        private readonly MoodEntry _editingMood;

        private int _selectedUserId = 0;

        public ManageMoodDetailPage(MoodEntry mood)
        {
            InitializeComponent();
            _editingMood = mood;

            LoadUsers();
            LoadExistingValues();
            NavigationPage.SetHasBackButton(this, false);
        }
        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminDashboardPage());
        }

        private async void LoadUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            UserPicker.ItemsSource = users
                .Select(u => $"{u.Id}: {u.FullName}")
                .ToList();

            // Pre-select user when editing
            if (_editingMood != null)
            {
                var match = $"{_editingMood.UserId}: {users.FirstOrDefault(u => u.Id == _editingMood.UserId)?.FullName}";
                UserPicker.SelectedItem = match;
                _selectedUserId = _editingMood.UserId;
            }

            // Store actual user ID when selected
            UserPicker.SelectedIndexChanged += (s, e) =>
            {
                if (UserPicker.SelectedIndex == -1)
                    return;

                var selected = UserPicker.SelectedItem.ToString();
                _selectedUserId = int.Parse(selected.Split(':')[0].Trim());
            };
        }

        private void LoadExistingValues()
        {
            DateEntry.MaximumDate = DateTime.Today; // Disable future dates

            if (_editingMood != null)
            {
                MoodPicker.SelectedItem = _editingMood.Mood;
                NoteEntry.Text = _editingMood.Note;
                DateEntry.Date = _editingMood.Date;
            }
            else
            {
                DateEntry.Date = DateTime.Today; // Default
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Mood validation
            if (MoodPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Error", "Please select a mood.", "OK");
                return;
            }

            string mood = MoodPicker.SelectedItem.ToString();

            // Note validation
            if (string.IsNullOrWhiteSpace(NoteEntry.Text) || NoteEntry.Text.Length < 10)
            {
                await DisplayAlert("Error", "Note must be at least 10 characters.", "OK");
                return;
            }

            // User validation
            if (_selectedUserId == 0)
            {
                await DisplayAlert("Error", "Please select a user.", "OK");
                return;
            }

            var moodEntry = new MoodEntry
            {
                Id = _editingMood?.Id ?? 0,
                Mood = mood,
                Note = NoteEntry.Text,
                Date = DateEntry.Date,
                UserId = _selectedUserId
            };

            bool success = _editingMood == null
                ? await _moodService.CreateMoodAsync(moodEntry)
                : await _moodService.UpdateMoodAsync(moodEntry);

            if (success)
            {
                await DisplayAlert("Success", "Mood saved successfully!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Failed to save mood.", "OK");
            }
        }
    }
}