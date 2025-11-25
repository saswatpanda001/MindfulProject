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
    public partial class MoodHistoryPage : ContentPage
    {
        private readonly MoodService _moodService = new MoodService();

        public MoodHistoryPage()
        {
            InitializeComponent();
            LoadMoods();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserDashboardPage());

        }
        private async void LoadMoods()
        {
            var user = SessionManager.LoggedInUser;

            if (user == null)
            {
                await DisplayAlert("Error", "User session expired.", "OK");
                return;
            }

            // Get all moods from API
            var allMoods = await _moodService.GetAllMoodsAsync();

            // Filter for logged-in user
            var myMoods = allMoods
                            .Where(m => m.UserId == user.Id)
                            .OrderByDescending(m => m.Date)
                            .ToList();

            MoodListView.ItemsSource = myMoods;
        }
    }
}