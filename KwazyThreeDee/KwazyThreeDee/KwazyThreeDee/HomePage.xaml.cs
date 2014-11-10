using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace KwazyThreeDee
{
    public partial class HomePage : ContentPage, ICommand
    {
        public HomePage()
        {
            InitializeComponent();
            return;
        }

        // Implementation of ICommand interface.
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            await Navigation.PushAsync(new DisplayPage((string)parameter));
        }
    }
}
