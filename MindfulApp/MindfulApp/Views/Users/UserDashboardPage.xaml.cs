using MindfulApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindfulApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindfulApp.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserDashboardPage : ContentPage
    {
        public UserDashboardPage()
        {
            InitializeComponent();

            // Set welcome message dynamically
            string userName = SessionManager.LoggedInUser?.FullName ?? "User";
            WelcomeLabel.Text = $"Welcome, {userName}!";
            NavigationPage.SetHasBackButton(this, false);
        }


        private async void OnMoodTrackerTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MoodTrackerPage());
        }

        private async void OnMeditationTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MeditationPage());
        }

        private async void OnAffirmationsTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateAffirmationPage());
        }

        private async void OnProfileTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage(SessionManager.LoggedInUser));
        }

        private async void OnResetPasswordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PasswordResetPage());
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            
            SessionManager.LoggedInUser = null;
            await Navigation.PushAsync(new LandingPage());
        }


    }
}