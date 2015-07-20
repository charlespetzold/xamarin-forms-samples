using System;
using Xamarin.Forms;

namespace KwazyThreeDee
{
    // A BoxView that is actually a line defined by two points and a thickness.
    // Requires AbsoluteLayout parent.
    class LineView : BoxView
    {
        public static readonly BindableProperty Point1Property =
            BindableProperty.Create("Point1", 
                                    typeof(Point), 
                                    typeof(LineView), 
                                    new Point(), 
                                    propertyChanged: OnPropertyChanged);

        public Point Point1
        {
            set { SetValue(Point1Property, value); }
            get { return (Point)GetValue(Point1Property); }
        }

        public static readonly BindableProperty Point2Property =
            BindableProperty.Create("Point2",
                                    typeof(Point),
                                    typeof(LineView),
                                    new Point(),
                                    propertyChanged: OnPropertyChanged);

        public Point Point2
        {
            set { SetValue(Point2Property, value); }
            get { return (Point)GetValue(Point2Property); }
        }

        public static readonly BindableProperty ThicknessProperty =
            BindableProperty.Create("Thickness",
                                    typeof(double),
                                    typeof(LineView),
                                    1.0,
                                    propertyChanged: OnPropertyChanged);

        public double Thickness
        {
            set { SetValue(ThicknessProperty, value); }
            get { return (double)GetValue(ThicknessProperty); }
        }

        static void OnPropertyChanged(BindableObject bindable, object oldValue, object NewValue)
        {
            ((LineView)bindable).OnPropertyChanged();
        }

        void OnPropertyChanged()
        {
            double length = Math.Sqrt(Math.Pow(Point2.X - Point1.X, 2) +
                                      Math.Pow(Point2.Y - Point1.Y, 2));

            double rotation = 180 * Math.Atan2(Point2.Y - Point1.Y,
                                               Point2.X - Point1.X) / Math.PI;

            // Avoid using AnchorX and AnchorY on iOS.
            if (Device.OS == TargetPlatform.iOS)
            {
                Point midPoint = new Point((Point1.X + Point2.X) / 2,
                                           (Point1.Y + Point2.Y) / 2);

                Point position = new Point(midPoint.X - length / 2, midPoint.Y - Thickness / 2);

                BatchBegin();
                AbsoluteLayout.SetLayoutBounds(this, new Rectangle(position, new Size(length, Thickness)));
                Rotation = rotation;
                BatchCommit();
            }
            else
            {
                AbsoluteLayout.SetLayoutBounds(this, new Rectangle(Point1, new Size(length, Thickness)));
                AnchorX = 0;
                AnchorY = 0;
                Rotation = rotation;
            }
        }
    }
}
