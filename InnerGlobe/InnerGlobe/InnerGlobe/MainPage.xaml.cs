using System;
using System.Numerics;

using Xamarin.Forms;
using Xamarin.Essentials;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using CoordinatesHelpers;

namespace InnerGlobe
{
    public partial class MainPage : ContentPage
    {
        const float UNITS_PER_DEGREE = 10;
        const float UNITS_FONT_SIZE = 18;

        float pixelsPerDegree;
        string[] compass = { "North", "NE", "East", "SE", "South", "SW", "West", "NW" };

        Quaternion orientation;
        HorizontalCoordinateProjection coordinateProjection = new HorizontalCoordinateProjection();

        SKPaint linePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            StrokeWidth = 3
        };

        SKPaint textPaint = new SKPaint
        {
            Color = SKColors.Black
        };

        public MainPage()
        {
            InitializeComponent();

            // Keep track of screen density
            pixelsPerDegree = (float)DeviceDisplay.ScreenMetrics.Density * UNITS_PER_DEGREE;
            textPaint.TextSize = (float)DeviceDisplay.ScreenMetrics.Density * UNITS_FONT_SIZE;

            DeviceDisplay.ScreenMetricsChanaged += (args) =>
            {
                pixelsPerDegree = (float)args.Metrics.Density * UNITS_PER_DEGREE;
                textPaint.TextSize = (float)args.Metrics.Density * UNITS_FONT_SIZE;
            };

            // Keep track of device orientation
            OrientationSensor.ReadingChanged += (args) =>
            {
                orientation = args.Reading.Orientation;
                canvasView.InvalidateSurface();
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

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

            // From orientation quaternion, get view center in horizontal coordinates
            Matrix4x4 matrix = Matrix4x4.CreateFromQuaternion(orientation);
            HorizontalCoordinate viewCenter = HorizontalCoordinate.FromRotationMatrix(matrix);
            coordinateProjection.SetViewCenter(viewCenter);

            // Rotate for Tilt
            canvas.RotateDegrees((float)-viewCenter.Tilt, info.Width / 2, info.Height / 2);

            // Draw aximuth lines
            for (double azimuth = 0; azimuth < 360; azimuth += 15)
            {
                for (double altitude = -90; altitude < 90; altitude += 15)
                {
                    HorizontalCoordinate coord1 = new HorizontalCoordinate(azimuth, altitude);
                    HorizontalCoordinate coord2 = new HorizontalCoordinate(azimuth, altitude + 15);

                    SKPoint point1 = CalculateScreenPoint(coord1, info.Width, info.Height);
                    SKPoint point2 = CalculateScreenPoint(coord2, info.Width, info.Height);

                    if (!double.IsNaN(point1.X) && !double.IsNaN(point2.X))
                    {
                        canvas.DrawLine(point1, point2, linePaint);
                    }
                }
            }

            // Draw altitude lines
            for (double altitude = -75; altitude < 90; altitude += 15)
            {
                for (double azimuth = 0; azimuth < 360; azimuth += 15)
                {
                    HorizontalCoordinate coord1 = new HorizontalCoordinate(azimuth, altitude);
                    HorizontalCoordinate coord2 = new HorizontalCoordinate(azimuth + 15, altitude);

                    SKPoint point1 = CalculateScreenPoint(coord1, info.Width, info.Height);
                    SKPoint point2 = CalculateScreenPoint(coord2, info.Width, info.Height);

                    if (!double.IsNaN(point1.X) && !double.IsNaN(point2.X))
                    {
                        canvas.DrawLine(point1, point2, linePaint);
                    }
                }
            }

            // Draw text for altitude angles
            for (double altitude = -75; altitude < 90; altitude += 15)
            {
                HorizontalCoordinate coord = new HorizontalCoordinate(viewCenter.Azimuth, altitude);
                SKPoint point = CalculateScreenPoint(coord, info.Width, info.Height);
                textPaint.TextAlign = SKTextAlign.Center;
                canvas.DrawText(altitude == 0 ? "Equator" : (altitude.ToString() + '\xB0'), point, textPaint);
            }

            // Draw text for azimuth compass points
            for (double azimuth = 0; azimuth < 360; azimuth += 45)
            {
                double altitude = Math.Min(80, Math.Max(-80, viewCenter.Altitude));
                HorizontalCoordinate coord = new HorizontalCoordinate(azimuth, altitude);
                SKPoint point = CalculateScreenPoint(coord, info.Width, info.Height);
                textPaint.TextAlign = SKTextAlign.Left;
                canvas.DrawText(compass[(int)(azimuth / 45)], point, textPaint);
            }
        }

        SKPoint CalculateScreenPoint(HorizontalCoordinate horizontalCoordinate, int width, int height)
        {
            coordinateProjection.GetAngleOffsets(horizontalCoordinate, out double horzAngle, out double vertAngle);

            // Use NaN to indicate points clearly out of range of the screen
            float x = float.NaN;
            float y = float.NaN;

            if (horzAngle > -90 && horzAngle < 90 && vertAngle > -90 && vertAngle < 90)
            {
                x = (float)(width / 2.0 + pixelsPerDegree * horzAngle);
                y = (float)(height / 2.0 + pixelsPerDegree * vertAngle);
            }
            return new SKPoint(x, y);
        }
    }
}
