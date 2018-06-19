using System;
using System.IO;
using System.Net.Http;
using System.Numerics;

using Xamarin.Forms;
using Xamarin.Essentials;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using CoordinatesHelpers;

namespace EarthlyDelights
{
	public partial class MainPage : ContentPage
	{
        const string url = "http://upload.wikimedia.org/wikipedia/commons/6/62/The_Garden_of_Earthly_Delights_by_Bosch_High_Resolution_2.jpg";

        SKBitmap bitmap;
        Quaternion orientation;

        public MainPage()
		{
			InitializeComponent();

            OrientationSensor.ReadingChanged += (args) =>
            {
                orientation = args.Reading.Orientation;
                canvasView.InvalidateSurface();
            };
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            HttpClient httpClient = new HttpClient();

            using (Stream stream = await httpClient.GetStreamAsync(url))
            using (MemoryStream memStream = new MemoryStream())
            {
                stream.CopyTo(memStream);
                memStream.Seek(0, SeekOrigin.Begin);

                using (SKManagedStream skStream = new SKManagedStream(memStream))
                {
                    bitmap = SKBitmap.Decode(skStream);


                    System.Diagnostics.Debug.WriteLine("{0} {1}", bitmap.Width, bitmap.Height);


                    canvasView.InvalidateSurface();
                }
            }

            try
            {
                OrientationSensor.Start(SensorSpeed.Normal);
            }
            catch
            {
                Content = new Label
                {
                    Text = "Sorry, the OrientationSensor is not supported on this device.",
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(50)
                };
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            OrientationSensor.Stop();
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            if (bitmap == null)
                return;

            Matrix4x4 matrix = Matrix4x4.CreateFromQuaternion(orientation);
            HorizontalCoordinate horzCoord = HorizontalCoordinate.FromRotationMatrix(matrix);

            float maxDimension = Math.Max(info.Width, info.Height);

            float bitmapCenterX = (bitmap.Width + maxDimension) * (180 + (float)horzCoord.Azimuth) / 360 - maxDimension / 2;
            float bitmapCenterY = (bitmap.Height + maxDimension) * (90 - (float)horzCoord.Altitude) / 180 - maxDimension / 2;

            float borderTransX = bitmapCenterX - info.Width / 2;
            float borderTransY = bitmapCenterY - info.Height / 2;

            float rotation = (float)-horzCoord.Tilt;

            float zoomInScale = Math.Min(info.Width - bitmap.Width, info.Height / bitmap.Height);


            /*
                       

                        canvas.Translate(info.Width / 2 - bitmapCenterX, info.Height / 2 - bitmapCenterY);

                        canvas.Scale(1);

                        canvas.RotateDegrees(rotation, bitmapCenterX, bitmapCenterY);
            */

            float scale = Math.Min(info.Width / (float)bitmap.Height, info.Height / (float)bitmap.Width);


            canvas.Translate((info.Width - scale * bitmap.Width) / 2, 
                             (info.Height - scale * bitmap.Height) / 2);


            canvas.Scale(scale);



            canvas.RotateDegrees(90, info.Width / 2, info.Height / 2);

            canvas.DrawBitmap(bitmap, new SKPoint(0, 0));
        }
    }
}
