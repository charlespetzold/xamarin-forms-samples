using System;
using System.Linq;
using Xamarin.Forms;
using Forms3D;
using System.Diagnostics;

namespace KwazyThreeDee
{
    class DisplayPage : ContentPage
    {
        Stopwatch stopwatch = new Stopwatch();
        bool stopAnimation = false;

        // Argument is SharedLineMesh derivative in Forms3D
        public DisplayPage(Type figureType)
        {
            Title = figureType.Name;

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
            stopwatch.Start();

            Device.StartTimer(TimeSpan.FromMilliseconds(16), () =>
            {
                double totalSeconds = stopwatch.Elapsed.TotalSeconds;

                Matrix4D matX = Matrix4D.RotationX(Math.PI * totalSeconds / 5);
                Matrix4D matY = Matrix4D.RotationY(Math.PI * totalSeconds / 3);
                Matrix4D matZ = Matrix4D.RotationZ(Math.PI * totalSeconds / 7);
                Matrix4D matView = Matrix4D.OrthographicView(new Point(this.Width / 2, this.Height / 2),
                                                             Math.Min(this.Width, this.Height) / 4);

                // Get composite 3D transform matrix.
                Matrix4D matrix = matX * matY * matZ * matView;

                // Loop through the shared lines that comprise the figure.
                for (int i = 0; i < sharedLines.Length; i++)
                {
                    // Transform the points and normals of those points. 
                    Point3D point1 = sharedLines[i].Point1 * matrix;
                    Point3D point2 = sharedLines[i].Point2 * matrix;
                    Vector3D normal1 = (Point3D)((Point3D)(sharedLines[i].Normal1) * matrix);
                    Vector3D normal2 = (Point3D)((Point3D)(sharedLines[i].Normal2) * matrix);

                    // Set each LineView to its new position.
                    lineViews[i].Point1 = point1;
                    lineViews[i].Point2 = point2;

                    // Determine if the line is visible or hidden.
                    bool isFacing = normal1.Z > 0 || normal2.Z > 0;
                    lineViews[i].Color = isFacing ? Color.Accent : Color.FromRgba(0.75, 0.75, 0.75, 0.5);
                }

                return !stopAnimation;
            });
        }

        protected override void OnDisappearing()
        {
            stopwatch.Stop();
            stopAnimation = true;
            base.OnDisappearing();
        }
    }
}
