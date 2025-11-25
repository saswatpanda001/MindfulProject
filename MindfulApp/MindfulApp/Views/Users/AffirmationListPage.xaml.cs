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
    public partial class AffirmationListPage : ContentPage
    {
        private readonly AffirmationService _affirmationService = new AffirmationService();

        public AffirmationListPage()
        {
            InitializeComponent();
            LoadAffirmations();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void LoadAffirmations()
        {
            var user = SessionManager.LoggedInUser;
            var allAffirmations = await _affirmationService.GetAllAffirmationsAsync();

            var userAffirmations = allAffirmations
                .Where(a => a.UserId == user.Id)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            AffirmationCollection.ItemsSource = userAffirmations;
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserDashboardPage());
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            int id = (int)btn.CommandParameter;

            bool confirm = await DisplayAlert("Confirm", "Delete this affirmation?", "Yes", "No");
            if (!confirm) return;

            bool success = await _affirmationService.DeleteAffirmationAsync(id);

            if (success)
            {
                await DisplayAlert("Deleted", "Affirmation removed.", "OK");
                LoadAffirmations();
            }
            else
            {
                await DisplayAlert("Error", "Failed to delete.", "OK");
            }
        }
    }
}