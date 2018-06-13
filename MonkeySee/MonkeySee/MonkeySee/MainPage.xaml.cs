using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Urho;
using Urho.Forms;

namespace MonkeySee
{
	public partial class MainPage : ContentPage
	{
        UrhoSurface urhoSurface;
        MonkeyDo monkeyDo;

		public MainPage()
		{
			InitializeComponent();

            urhoSurface = new UrhoSurface();
            Content = urhoSurface;

            OrientationSensor.ReadingChanged += (args) =>
            {
                System.Numerics.Quaternion q = args.Reading.Orientation;
                monkeyDo.Orientation = new Quaternion(q.X, q.Z, q.Y, q.W);


                return;

                Urho.Quaternion urhoQ = new Quaternion(q.X, q.Y, q.Z, q.W);

                quaternion.Text = String.Format("{0:F3} {1:F3} {2:F3} {3:F3}", q.X, q.Y, q.Z, q.W);

                q = System.Numerics.Quaternion.Normalize(q);

                normalized.Text = String.Format("{0:F3} {1:F3} {2:F3} {3:F3}", q.X, q.Y, q.Z, q.W);

                double a = 2 * Math.Acos(q.W);

                float sine = (float)Math.Sin(a / 2);

                System.Numerics.Vector3 v = new System.Numerics.Vector3(q.X * sine, q.Y * sine, q.Z * sine);

                axis.Text = String.Format("{0:F2} {1:F2} {2:F2}", v.X, v.Y, v.Z);

                angle.Text = (180 * a / Math.PI).ToString("F0");

                v = System.Numerics.Vector3.Normalize(v);

                normAxis.Text = String.Format("{0:F2} {1:F2} {2:F2}", v.X, v.Y, v.Z);

                urhoQ.ToAxisAngle(out Vector3 axisUrho, out float angleUrho);

                urhoAxis.Text = String.Format("{0:F2} {1:F2} {2:F2}", axisUrho.X, axisUrho.Y, axisUrho.Z);

                urhoAngle.Text = angleUrho.ToString("F3");





            };



        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            monkeyDo = await urhoSurface.Show<MonkeyDo> (new ApplicationOptions(assetsFolder: "Data")
            {
                Orientation = ApplicationOptions.OrientationType.Portrait
            });

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
            UrhoSurface.OnDestroy();
        }
    }
}
