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
    public partial class MeditationSessionHistoryPage : ContentPage
    {
        private readonly MeditationService _meditationService = new MeditationService();

        public MeditationSessionHistoryPage()
        {
            InitializeComponent();
            LoadSessions();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserDashboardPage());
        }

        private async void LoadSessions()
        {
            var user = SessionManager.LoggedInUser;

            if (user == null)
            {
                await DisplayAlert("Error", "No logged-in user found.", "OK");
                return;
            }

            // Fetch everything
            var allSessions = await _meditationService.GetAllSessionsAsync();

            // Filter by logged in user
            var userSessions = allSessions
                .Where(s => s.UserId == user.Id)
                .OrderByDescending(s => s.Date)
                .ToList();

            SessionListView.ItemsSource = userSessions;
        }
    }
}
