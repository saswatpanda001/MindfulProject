using MindfulApp.Models;
using MindfulApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindfulApp.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoodTrackerPage : ContentPage
    {
        public MoodTrackerPage()
        {
            InitializeComponent();

            // Set logged-in user
            UserLabel.Text = $"User: {SessionManager.LoggedInUser?.FullName}";

            // Set date
            DateLabel.Text = DateTime.Now.ToString("dd MMM yyyy");

            NavigationPage.SetHasBackButton(this, false);
        }


        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserDashboardPage());
        }

        private readonly MoodService _moodService = new MoodService();

        private async void OnSaveMoodClicked(object sender, EventArgs e)
        {
            if (MoodPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please select a mood.", "OK");
                return;
            }

            if (SessionManager.LoggedInUser == null)
            {
                await DisplayAlert("Error", "User session expired.", "OK");
                return;
            }

            string mood = MoodPicker.SelectedItem.ToString();

            // Only allow: Great, Good, Okay, Low, Sad
            string[] validMoods = { "Great", "Good", "Okay", "Low", "Sad" };

            if (!validMoods.Contains(mood))
            {
                await DisplayAlert("Error", "Invalid mood selected.", "OK");
                return;
            }

            // Validate note minimum 4 chars
            string notes = NotesEditor.Text?.Trim() ?? "";

            if (!string.IsNullOrEmpty(notes) && notes.Length < 4)
            {
                await DisplayAlert("Error", "Note must be at least 4 characters long.", "OK");
                return;
            }

            string date = DateLabel.Text;
            int userId = SessionManager.LoggedInUser.Id;

            // Create mood entry object
            var moodEntry = new MoodEntry
            {
                Mood = mood,
                Note = notes,
                Date = DateTime.Now,
                UserId = userId
            };

            // Call API
            bool saved = await _moodService.CreateMoodAsync(moodEntry);

            if (saved)
            {
                await DisplayAlert("Success", "Your mood has been saved!", "OK");

                // Reset UI
                MoodPicker.SelectedIndex = -1;
                NotesEditor.Text = string.Empty;
                await Navigation.PushAsync(new MoodHistoryPage());
            }
            else
            {
                await DisplayAlert("Error", "Failed to save mood. Please try again.", "OK");
            }
        }
        private async void OnMoodRecordsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MoodHistoryPage());


        }

        
    }
}