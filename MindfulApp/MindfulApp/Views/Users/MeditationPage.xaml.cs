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
    public partial class MeditationPage : ContentPage
    {
        private bool _isRunning = false;
        private DateTime _startTime;
        private TimeSpan _elapsed = TimeSpan.Zero;
        private readonly MeditationService _service = new MeditationService();

        public MeditationPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }
        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserDashboardPage());
        }

        private void OnPlayPauseTapped(object sender, EventArgs e)
        {
            if (!_isRunning)
            {
                // Start timer
                _isRunning = true;
                _startTime = DateTime.Now;
                PlayPauseLabel.Text = "⏸";
                Device.StartTimer(TimeSpan.FromSeconds(1), TimerTick);
            }
            else
            {
                // Pause timer
                _isRunning = false;
                _elapsed += DateTime.Now - _startTime;
                PlayPauseLabel.Text = "▶";
            }
        }

        private bool TimerTick()
        {
            if (_isRunning)
            {
                var current = _elapsed + (DateTime.Now - _startTime);
                TimeLabel.Text = current.ToString(@"mm\:ss");
            }
            return _isRunning; // keep timer running
        }

        private void OnResetTapped(object sender, EventArgs e)
        {
            _isRunning = false;
            _elapsed = TimeSpan.Zero;
            TimeLabel.Text = "00:00";
            PlayPauseLabel.Text = "▶";
        }

        private async void OnStopTapped(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                _elapsed += DateTime.Now - _startTime;
                _isRunning = false;
            }

            int durationMinutes = (int)Math.Round(_elapsed.TotalMinutes);

            if (durationMinutes <= 0)
            {
                await DisplayAlert("Alert", "Meditation duration too short.", "OK");
                return;
            }

            int userId = SessionManager.LoggedInUser.Id;

            var session = new MeditationSession
            {
                DurationMinutes = durationMinutes,
                Date = DateTime.Now,
                UserId = userId
            };

            bool saved = await _service.CreateSessionAsync(session);

            if (saved)
            {
                await DisplayAlert("Success", $"Session saved ({durationMinutes} minutes)", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to save session.", "OK");
            }

            // Reset after save
            _elapsed = TimeSpan.Zero;
            TimeLabel.Text = "00:00";
            PlayPauseLabel.Text = "▶";
        }

        private async void OnMeditationSessionsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MeditationSessionHistoryPage());


        }


       
    }
}