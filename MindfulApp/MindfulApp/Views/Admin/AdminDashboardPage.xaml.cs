using MindfulApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using Xamarin.Essentials;


namespace MindfulApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminDashboardPage : ContentPage
    {
        private readonly UserService _userService = new UserService();
        private readonly MoodService _moodService = new MoodService();
        private readonly MeditationService _meditationService = new MeditationService();
        private readonly AffirmationService _affirmationService = new AffirmationService();

        public AdminDashboardPage()
        {
            InitializeComponent();
            SetWelcomeMessage();
            _ = LoadStatisticsAsync();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LandingPage());
        }

        private async Task LoadStatisticsAsync()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                var moods = await _moodService.GetAllMoodsAsync();
                var sessions = await _meditationService.GetAllSessionsAsync();
                var affirmations = await _affirmationService.GetAllAffirmationsAsync();

                TotalUsersLabel.Text = (users?.Count ?? 0).ToString();
                TotalMoodEntriesLabel.Text = (moods?.Count ?? 0).ToString();
                TotalMeditationSessionsLabel.Text = (sessions?.Count ?? 0).ToString();
                TotalAffirmationsLabel.Text = (affirmations?.Count ?? 0).ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load statistics: {ex.Message}", "OK");
            }
        }

        private void SetWelcomeMessage()
        {
            var user = SessionManager.LoggedInUser;

            if (user != null)
            {
                WelcomeLabel.Text = $"Welcome, {user.FullName}";
            }
            else
            {
                WelcomeLabel.Text = "Welcome, Admin";
            }
        }



        private async void OnManageUsersClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageUserPage());
        }

        private async void OnMoodEntriesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageMoodPage());
        }

        private async void OnMeditationSessionsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageMeditationPage());
        }

        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage(SessionManager.LoggedInUser));
        }

        private async void OnResetPasswordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PasswordResetPage());
        }

        

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");

            if (confirm)
            {
                SessionManager.LoggedInUser = null;
                await Navigation.PushAsync(new LandingPage());
            }
        }



        private async void OnGenerateReportClicked(object sender, EventArgs e)
        {
            try
            {
                // 1. Fetch data using your services
                var users = await _userService.GetAllUsersAsync();
                var moods = await _moodService.GetAllMoodsAsync();
                var sessions = await _meditationService.GetAllSessionsAsync();
                var affirmations = await _affirmationService.GetAllAffirmationsAsync();

                // 2. Build the report using StringBuilder
                var sb = new StringBuilder();
                sb.AppendLine("===== MINDFUL APP REPORT =====");
                sb.AppendLine($"Generated On: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"Total Users: {users?.Count ?? 0}");
                sb.AppendLine($"Total Mood Entries: {moods?.Count ?? 0}");
                sb.AppendLine($"Total Meditation Sessions: {sessions?.Count ?? 0}");
                sb.AppendLine($"Total Affirmations: {affirmations?.Count ?? 0}");
                sb.AppendLine();

                // Users Section
                sb.AppendLine("----- Users -----");
                if (users != null && users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        sb.AppendLine($"ID: {user.Id}, Name: {user.FullName}, Email: {user.Email}, Role: {user.Role}");
                    }
                }
                else
                {
                    sb.AppendLine("No users available.");
                }

                sb.AppendLine();

                // Mood Entries Section
                sb.AppendLine("----- Mood Entries -----");
                if (moods != null && moods.Count > 0)
                {
                    foreach (var mood in moods)
                    {
                        sb.AppendLine($"ID: {mood.Id}, UserID: {mood.UserId}, Mood: {mood.Mood}, Note: {mood.Note}, Date: {mood.Date:yyyy-MM-dd}");
                    }
                }
                else
                {
                    sb.AppendLine("No mood entries available.");
                }

                sb.AppendLine();

                // Meditation Sessions Section
                sb.AppendLine("----- Meditation Sessions -----");
                if (sessions != null && sessions.Count > 0)
                {
                    foreach (var session in sessions)
                    {
                        sb.AppendLine($"ID: {session.Id}, UserID: {session.UserId}, Duration: {session.DurationMinutes} min, Date: {session.Date:yyyy-MM-dd}");
                    }
                }
                else
                {
                    sb.AppendLine("No meditation sessions available.");
                }

                sb.AppendLine();

                // Affirmations Section
                sb.AppendLine("----- Affirmations -----");
                if (affirmations != null && affirmations.Count > 0)
                {
                    foreach (var affirmation in affirmations)
                    {
                        sb.AppendLine($"ID: {affirmation.Id}, UserID: {affirmation.UserId}, Category: {affirmation.Category}, Text: {affirmation.Text}, Date: {affirmation.CreatedAt:yyyy-MM-dd}");
                    }
                }
                else
                {
                    sb.AppendLine("No affirmations available.");
                }

                // 3. Save the report locally
                var fileName = $"MindfulApp_Report_{DateTime.Now:yyyyMMddHHmmss}.txt";
                var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var filePath = Path.Combine(folderPath, fileName);
                File.WriteAllText(filePath, sb.ToString());

                // 4. Optionally share the report
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Mindful App Report",
                    File = new ShareFile(filePath)
                });

                // 5. Confirmation
                await DisplayAlert("Report Generated", $"Report saved successfully at:\n{filePath}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to generate report: " + ex.Message, "OK");
            }
        }


    }
}