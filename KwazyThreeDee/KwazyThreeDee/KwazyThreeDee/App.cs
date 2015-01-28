using System;
using Xamarin.Forms;

namespace KwazyThreeDee
{
    public class App : Application
    {
        public App()
        {
            // Get reference to library
            var x = new Forms3D.Matrix4D();

            MainPage = new NavigationPage(new HomePage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
