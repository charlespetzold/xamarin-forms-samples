using System;
using Xamarin.Forms;

namespace PageFlipper
{
    public partial class PageFlipperPage : ContentPage
    {
        readonly int pageCount;
        int pageNumber = 0;

        public PageFlipperPage()
        {
            InitializeComponent();
            pageCount = pageGrid.Children.Count;
        }

        async void OnBackClicked(object sender, EventArgs args)
        {
            pageNumber--;
            View page = pageGrid.Children[pageCount - pageNumber - 1];
            EnableDisableButtons();

            page.AnchorX = 0;
            page.AnchorY = 0.51;
            await page.RotateYTo(0, 500);
        }

        async void OnForwardClicked(object sender, EventArgs args)
        {
            View page = pageGrid.Children[pageCount - pageNumber - 1];
            pageNumber++;
            EnableDisableButtons();

            page.AnchorX = 0;
            page.AnchorY = 0.51;
            await page.RotateYTo(-90, 500);

            // On Android sometimes the next page doesn't show up
            //      until the button is clicked to go forward again,
            //      so try to fix that.
            Device.OnPlatform(null, async () =>
                {
                    page = pageGrid.Children[pageCount - pageNumber - 1];
                    await page.RotateYTo(10, 15);
                    await page.RotateYTo(0, 15);
                });
        }

        void EnableDisableButtons()
        {
            backButton.IsEnabled = pageNumber > 0;
            forwardButton.IsEnabled = pageNumber < pageCount - 1;
        }
    }
}
