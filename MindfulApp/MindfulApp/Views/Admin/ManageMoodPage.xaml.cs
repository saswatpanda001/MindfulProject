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
    public partial class ManageMoodPage : ContentPage
    {
        private readonly MoodService _moodService = new MoodService();

        public ManageMoodPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadMoodsAsync();
        }
        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminDashboardPage());
        }

        private async System.Threading.Tasks.Task LoadMoodsAsync()
        {
            var moods = await _moodService.GetAllMoodsAsync();
            MoodListView.ItemsSource = moods;
        }

        private async void OnAddNewMoodClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageMoodDetailPage(null));
        }

        private async void OnEditMoodClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int moodId)
            {
                var moods = (MoodListView.ItemsSource as System.Collections.IEnumerable)?.Cast<MoodEntry>();
                var selectedMood = moods?.FirstOrDefault(m => m.Id == moodId);
                if (selectedMood != null)
                {
                    await Navigation.PushAsync(new ManageMoodDetailPage(selectedMood));
                }
            }
        }

        private async void OnDeleteMoodClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int moodId)
            {
                bool confirm = await DisplayAlert("Confirm", "Are you sure you want to delete this mood?", "Yes", "No");
                if (confirm)
                {
                    bool isDeleted = await _moodService.DeleteMoodAsync(moodId);
                    if (isDeleted)
                    {
                        await DisplayAlert("Success", "Mood deleted successfully.", "OK");
                        await LoadMoodsAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to delete mood.", "OK");
                    }
                }
            }
        }
    }
}