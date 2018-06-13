using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

using Xamarin.Forms;
using Xamarin.Essentials;

using Xamarin.FormsBook.Platform;

namespace TiltMaze
{
	public partial class MainPage : ContentPage
	{
        const float GRAVITY = 1000;     // pixels per second squared
        const float BOUNCE = 2f / 3;    // fraction of velocity

        const int MAZE_HORZ_CHAMBERS = 5;
        const int MAZE_VERT_CHAMBERS = 8;

        const int WALL_WIDTH = 16;
        const int BALL_RADIUS = 12;
        const int HOLE_RADIUS = 18;

        EllipseView ball;
        EllipseView hole;
        MazeGrid mazeGrid;
        List<Line2D> borders = new List<Line2D>();

        Random random = new Random();

        Stopwatch stopwatch = new Stopwatch();
        bool isBallInPlay = false;
        TimeSpan lastElapsedTime;

        Vector3 acceleration;
        Vector2 ballVelocity = Vector2.Zero;
        Vector2 ballPosition;
        Vector2 holePosition;

        public MainPage()
		{
			InitializeComponent();

            Accelerometer.ReadingChanged += (args) =>
            {
                // Smooth the reading by averaging with prior values
                acceleration = 0.5f * args.Reading.Acceleration + 0.5f * acceleration;
            };

            Device.StartTimer(TimeSpan.FromMilliseconds(33), () =>
            {
                TimeSpan elapsedTime = stopwatch.Elapsed;
                float deltaSeconds = (float)(elapsedTime - lastElapsedTime).TotalSeconds;
                lastElapsedTime = elapsedTime;

                if (isBallInPlay)
                {
                    // MoveBall returns true for end of game
                    if (MoveBall(deltaSeconds))
                    {
                        // Aysnchronous method
                        TransitionToNewGame();
                    }
                }
                return true;
            });
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                Accelerometer.Start(SensorSpeed.Normal);
            }
            catch
            {
                Label label = new Label
                {
                    Text = "Sorry, an accelerometer is not available on this device",
                    FontSize = 24,
                    TextColor = Color.White,
                    BackgroundColor = Color.DarkGray,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(48, 0)
                };

                absoluteLayout.Children.Add(label,
                      new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize), 
                      AbsoluteLayoutFlags.PositionProportional);
            }
            stopwatch.Start();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Accelerometer.Stop();
            stopwatch.Stop();
        }

        void OnAbsoluteLayoutSizeChanged(object sender, EventArgs args)
        {
            if (Width > 0 && Height > 0)
            {
                NewGame((float)absoluteLayout.Width, (float)absoluteLayout.Height);
                isBallInPlay = true;

                // Detach this handler to avoid interrupting games
                absoluteLayout.SizeChanged -= OnAbsoluteLayoutSizeChanged;
            }
        }

        async void TransitionToNewGame()
        {
            isBallInPlay = false;
            absoluteLayout.Children.Remove(ball);
            await absoluteLayout.FadeTo(0, 1000);
            NewGame((float)absoluteLayout.Width, (float)absoluteLayout.Height);
            await absoluteLayout.FadeTo(1, 500);
            isBallInPlay = true;
        }

        void NewGame(float width, float height)
        {
            // The constructor creates the random maze layout
            mazeGrid = new MazeGrid(MAZE_HORZ_CHAMBERS, MAZE_VERT_CHAMBERS);

            // Initialize borders collection
            borders.Clear();

            // Create Line2D objects for the border lines of the maze
            float cellWidth = width / mazeGrid.Width;
            float cellHeight = height / mazeGrid.Height;
            int halfWallWidth = WALL_WIDTH / 2;

            for (int x = 0; x < mazeGrid.Width; x++)
                for (int y = 0; y < mazeGrid.Height; y++)
                {
                    MazeCell mazeCell = mazeGrid.Cells[x, y];
                    Vector2 ll = new Vector2(x * cellWidth, (y + 1) * cellHeight);
                    Vector2 ul = new Vector2(x * cellWidth, y * cellHeight);
                    Vector2 ur = new Vector2((x + 1) * cellWidth, y * cellHeight);
                    Vector2 lr = new Vector2((x + 1) * cellWidth, (y + 1) * cellHeight);
                    Vector2 right = halfWallWidth * Vector2.UnitX;
                    Vector2 left = -right;
                    Vector2 down = halfWallWidth * Vector2.UnitY;
                    Vector2 up = -down;

                    if (mazeCell.HasLeft)
                    {
                        borders.Add(new Line2D(ll + down, ll + down + right));
                        borders.Add(new Line2D(ll + down + right, ul + up + right));
                        borders.Add(new Line2D(ul + up + right, ul + up));
                    }
                    if (mazeCell.HasTop)
                    {
                        borders.Add(new Line2D(ul + left, ul + left + down));
                        borders.Add(new Line2D(ul + left + down, ur + right + down));
                        borders.Add(new Line2D(ur + right + down, ur + right));
                    }
                    if (mazeCell.HasRight)
                    {
                        borders.Add(new Line2D(ur + up, ur + up + left));
                        borders.Add(new Line2D(ur + up + left, lr + down + left));
                        borders.Add(new Line2D(lr + down + left, lr + down));
                    }
                    if (mazeCell.HasBottom)
                    {
                        borders.Add(new Line2D(lr + right, lr + right + up));
                        borders.Add(new Line2D(lr + right + up, ll + left + up));
                        borders.Add(new Line2D(ll + left + up, ll + left));
                    }
                }

            // Prepare AbsoluteLayout for new children
            absoluteLayout.Children.Clear();

            // "Draw" the walls of the maze using BoxView
            BoxView createBoxView() => new BoxView { Color = Color.Green };

            for (int x = 0; x < mazeGrid.Width; x++)
                for (int y = 0; y < mazeGrid.Height; y++)
                {
                    MazeCell mazeCell = mazeGrid.Cells[x, y];

                    if (mazeCell.HasLeft)
                    {
                        Rectangle rect = new Rectangle(x * cellWidth,
                                                       y * cellHeight - halfWallWidth,
                                                       halfWallWidth, cellHeight + WALL_WIDTH);

                        absoluteLayout.Children.Add(createBoxView(), rect);
                    }

                    if (mazeCell.HasRight)
                    {
                        Rectangle rect = new Rectangle((x + 1) * cellWidth - halfWallWidth,
                                                       y * cellHeight - halfWallWidth,
                                                       halfWallWidth, cellHeight + WALL_WIDTH);

                        absoluteLayout.Children.Add(createBoxView(), rect);
                    }

                    if (mazeCell.HasTop)
                    {
                        Rectangle rect = new Rectangle(x * cellWidth - halfWallWidth,
                                                       y * cellHeight,
                                                       cellWidth + WALL_WIDTH, halfWallWidth);

                        absoluteLayout.Children.Add(createBoxView(), rect);
                    }

                    if (mazeCell.HasBottom)
                    {
                        Rectangle rect = new Rectangle(x * cellWidth - halfWallWidth,
                                                       (y + 1) * cellHeight - halfWallWidth,
                                                       cellWidth + WALL_WIDTH, halfWallWidth);

                        absoluteLayout.Children.Add(createBoxView(), rect);
                    }
                }

            // Randomly position ball in one of the corners
            bool isBallLeftCorner = random.Next(2) == 0;
            bool isBallTopCorner = random.Next(2) == 0;

            // Create the hole first (so Z order is under the ball) 
            //      and position it in the opposite corner from the ball
            hole = new EllipseView
            {
                Color = Color.Black,
                WidthRequest = 2 * HOLE_RADIUS,
                HeightRequest = 2 * HOLE_RADIUS
            };

            holePosition = new Vector2((isBallLeftCorner ? 2 * mazeGrid.Width - 1 : 1) * (width / mazeGrid.Width / 2),
                                       (isBallTopCorner ? 2 * mazeGrid.Height - 1 : 1) * (height / mazeGrid.Height / 2));

            absoluteLayout.Children.Add(hole, new Point(holePosition.X - HOLE_RADIUS, 
                                                        holePosition.Y - HOLE_RADIUS));

            // Create the ball and set initial position 
            ball = new EllipseView
            {
                Color = Color.Red,
                WidthRequest = 2 * BALL_RADIUS,
                HeightRequest = 2 * BALL_RADIUS
            };

            ballPosition = new Vector2((isBallLeftCorner ? 1 : 2 * mazeGrid.Width - 1) * (width / mazeGrid.Width / 2),
                                       (isBallTopCorner ? 1 : 2 * mazeGrid.Height - 1) * (height / mazeGrid.Height / 2));

            absoluteLayout.Children.Add(ball, new Point(ballPosition.X - BALL_RADIUS,
                                                        ballPosition.Y - BALL_RADIUS));
        }

        bool MoveBall(float deltaSeconds)
        {
            // Save the position just in case the velocity drops to zero
            Vector2 savedBallPosition = ballPosition;

            // Get acceleration, do the physics
            Vector2 acceleration2D = new Vector2(-acceleration.X, acceleration.Y);
            ballVelocity += GRAVITY * acceleration2D * deltaSeconds;
            Vector2 oldPosition = ballPosition;
            ballPosition += ballVelocity * deltaSeconds;

            // Loop through possible bounces
            bool needAnotherLoop = false;

            do
            {
                needAnotherLoop = false;

                foreach (Line2D line in borders)
                {
                    // Check if ball has crossed a line of the wall
                    Line2D shiftedLine = line.ShiftOut(BALL_RADIUS * line.Normal);
                    Line2D ballTrajectory = new Line2D(oldPosition, ballPosition);
                    Vector2 intersection = shiftedLine.SegmentIntersection(ballTrajectory);
                    float angleDiff = WrapAngle(line.Angle - ballTrajectory.Angle);

                    if (Line2D.IsValid(intersection) &&
                        angleDiff > 0 &&
                        Line2D.IsValid(Vector2.Normalize(ballVelocity)))
                    {
                        float beyond = (ballPosition - intersection).Length();
                        ballVelocity = BOUNCE * Vector2.Reflect(ballVelocity, line.Normal);

                        // Check if the magnitude of the ball velocity is zero
                        if (ballVelocity.Length() == 0)
                        {
                            // Just get out of here; danger lurks otherwise
                            ballPosition = savedBallPosition;
                            break;
                        }

                        // Set new ball position ofter bounce
                        ballPosition = intersection + beyond * Vector2.Normalize(ballVelocity);

                        // But the bounce might put it beyond another border line
                        needAnotherLoop = true;
                        break;
                    }
                }
            }
            while (needAnotherLoop);

            // Position the ball at ballPosition
            Rectangle ballRect = new Rectangle(ballPosition.X - BALL_RADIUS, 
                                               ballPosition.Y - BALL_RADIUS, 
                                               AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);

            AbsoluteLayout.SetLayoutBounds(ball, ballRect);

            // Return true for GAME OVER if the ball is within the hole 
            return (ballPosition - holePosition).Length() < HOLE_RADIUS - BALL_RADIUS;
        }

        // Forces angle between -PI and PI
        float WrapAngle(float angle)
        {
            const float pi = (float)Math.PI;

            angle = (float)Math.IEEERemainder(angle, 2 * pi);

            if (angle <= -pi)
            {
                angle += 2 * pi;
            }
            if (angle > pi)
            {
                angle -= 2 * pi;
            }
            return angle;
        }
    }
}
