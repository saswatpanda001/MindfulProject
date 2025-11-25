using MindfulApp.Services;
using MindfulApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindfulApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();


            MainPage = new NavigationPage(new LandingPage());
        }

    }
}
