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
    public partial class CreateAffirmationPage : ContentPage
    {
        private readonly AffirmationService _affirmationService = new AffirmationService();

        public CreateAffirmationPage()
        {
            InitializeComponent();
            DateEntry.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            NavigationPage.SetHasBackButton(this, false);
        }
        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserDashboardPage());
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (CategoryPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please pick a category.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(AffirmationText.Text))
            {
                await DisplayAlert("Error", "Affirmation text cannot be empty.", "OK");
                return;
            }

            var user = SessionManager.LoggedInUser;

            var newAffirmation = new AffirmationEntry
            {
                Category = CategoryPicker.SelectedItem.ToString(),
                Text = AffirmationText.Text.Trim(),
                CreatedAt = DateTime.Now,
                UserId = user.Id
            };

            bool success = await _affirmationService.CreateAffirmationAsync(newAffirmation);

            if (success)
            {
                await DisplayAlert("Success", "Affirmation saved!", "OK");
                await Navigation.PushAsync(new AffirmationListPage());
            }
            else
            {
                await DisplayAlert("Error", "Failed to save.", "OK");
            }
        }


        private async void OnAffirmationHistoryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AffirmationListPage());


        }
    }
}

