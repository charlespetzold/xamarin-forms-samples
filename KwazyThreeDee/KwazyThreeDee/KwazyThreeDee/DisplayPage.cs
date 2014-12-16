using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Forms3D;

namespace KwazyThreeDee
{
    class DisplayPage : ContentPage
    {
        bool stopAnimation = false;

        public DisplayPage(string strFigureType)
        {
            this.Title = strFigureType.Substring(strFigureType.IndexOf('.') + 1);

            // Get the PolyhedronBase object specified by the parameter.
            Assembly assembly = typeof(SharedLineMesh).GetTypeInfo().Assembly;
            Type figureType = assembly.GetType(strFigureType);
            SharedLineMesh mesh = (SharedLineMesh)Activator.CreateInstance(figureType);

            SharedLine[] sharedLines = mesh.SharedLines.ToArray();
            LineView[] lineViews = new LineView[sharedLines.Length];

            // Set up page with LineView elements.
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();

            for (int i = 0; i < lineViews.Length; i++)
            {
                lineViews[i] = new LineView
                {
                    Thickness = 3
                };
                absoluteLayout.Children.Add(lineViews[i]);
            }

            this.Content = absoluteLayout;

            // Run animation.
            DateTime dtStart = DateTime.Now;

            Device.StartTimer(TimeSpan.FromMilliseconds(16), () =>
            {
                TimeSpan timeSpan = DateTime.Now - dtStart;

                Matrix4D matX = Matrix4D.RotationX(Math.PI * timeSpan.TotalSeconds / 5);
                Matrix4D matY = Matrix4D.RotationY(Math.PI * timeSpan.TotalSeconds / 3);
                Matrix4D matZ = Matrix4D.RotationZ(Math.PI * timeSpan.TotalSeconds / 7);
                Matrix4D matView = Matrix4D.OrthographicView(new Point(this.Width / 2, this.Height / 2),
                                                             Math.Min(this.Width, this.Height) / 4);
                Matrix4D matrix = matX * matY * matZ * matView;

                for (int i = 0; i < sharedLines.Length; i++)
                {
                    Point3D point1 = sharedLines[i].Point1 * matrix;
                    Point3D point2 = sharedLines[i].Point2 * matrix;
                    Vector3D normal1 = (Point3D)((Point3D)(sharedLines[i].Normal1) * matrix);
                    Vector3D normal2 = (Point3D)((Point3D)(sharedLines[i].Normal2) * matrix);

                    lineViews[i].Point1 = point1;
                    lineViews[i].Point2 = point2;

                    bool isFacing = normal1.Z > 0 || normal2.Z > 0;
                    lineViews[i].Color = isFacing ? Color.Accent : Color.FromRgba(0.75, 0.75, 0.75, 0.5);
                }

                return !stopAnimation;
            });
        }

        protected override void OnDisappearing()
        {
            stopAnimation = true;
            base.OnDisappearing();
        }
    }
}
