using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RotatingSpokes
{
    class RotatingSpokesPage : ContentPage
    {
        const int numSpokes = 24;
        AbsoluteLayout absoluteLayout;
        BoxView[] boxViews = new BoxView[numSpokes];

        public RotatingSpokesPage()
        {
            this.BackgroundColor = Color.White;

            absoluteLayout = new AbsoluteLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            for (int i = 0; i < numSpokes; i++)
            {
                BoxView boxView = new BoxView
                {
                    Color = Color.Black
                };
                boxViews[i] = boxView;
                absoluteLayout.Children.Add(boxView);
            }

            this.Content = absoluteLayout;

            this.SizeChanged += OnPageSizeChanged;

            DoAnimations();
        }

        async void DoAnimations()
        {
            // Keep still for 3 seconds
            await Task.Delay(3000);

            // Rotate the configuration of spokes 3 times
            uint count = 3;
            absoluteLayout.AnchorX = 0.51;      // "fix" Android bug
            absoluteLayout.AnchorY = 0.51;
            await absoluteLayout.RotateTo(360 * count, 3000 * count);

            while (true)
            {
                for (int i = 0; i < numSpokes; i++)
                {
                    // Rotate each spoke without await-ing ...
                    boxViews[i].RelRotateTo(360, 3000);
                }

                // ... while rotating the whole configuration
                await absoluteLayout.RelRotateTo(360, 3000);
            }
        }

        void OnPageSizeChanged(object sender, EventArgs args)
        {
            double dimension = Math.Min(this.Width, this.Height);
            absoluteLayout.WidthRequest = dimension;
            absoluteLayout.HeightRequest = dimension;

            Point layoutCenter = new Point(dimension / 2, dimension / 2);
            Size boxViewSize = new Size(dimension / 2, 3);

            for (int i = 0; i < numSpokes; i++)
            {
                double degrees = i * 360 / numSpokes;
                double radians = Math.PI * degrees / 180;

                double boxViewRadius = boxViewSize.Width / 2;
                Point boxViewCenter = new Point(boxViewRadius * Math.Cos(radians),
                                                boxViewRadius * Math.Sin(radians));
                boxViewCenter.X += layoutCenter.X;
                boxViewCenter.Y += layoutCenter.Y;

                Point boxViewOrigin = boxViewCenter - boxViewSize * 0.5; 
                AbsoluteLayout.SetLayoutBounds(boxViews[i], new Rectangle(boxViewOrigin, boxViewSize));

                boxViews[i].AnchorX = 0.51;         // "fix" Android bug
                boxViews[i].AnchorY = 0.51;
                boxViews[i].Rotation = degrees;
            }
        }
    }
}
