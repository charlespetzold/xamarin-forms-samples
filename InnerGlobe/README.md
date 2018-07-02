Inner Globe
=========

**Inner Globe** is a program for Xamarin.Forms that uses the cross-platform [`OrientationSensor`](https://docs.microsoft.com/xamarin/essentials/orientation-sensor?context=xamarin/xamarin-forms) included in Xamarin.Essentials. 

**Inner Globe** runs on iOS, Android, and the Universal Windows Platform (UWP), but you'll need to run the program on an actual phone or tablet rather than a simulator. The program uses SkiaSharp graphics to display horizontal coordinates as if you were on the inside of a celestial sphere.

Horizontal coordinates are analogous to geographic coordinates: Instead of lines of latitude, you have lines of altitude -- the equator when the phone is pointing towards the horizon, and ranging up to 90 degrees at the top and bottom. Rather than lines of longitude, you have degrees of azimuth, which indicate the compass point to which the phone is pointing.

Xamarin.Forms no longer runs on Windows Phone or Windows 10 Mobile devices, but **Inner Globe** _will_ run on a Windows 10 tablet, such as a Surface Pro, and runs best in Tablet Mode. To run in Tablet Mode, all external monitors must be disconnected from the device. Sweep your finger along the right edge of the screen (or press the Notifications icon to the right of the date and time) to display the **Action Center**. Select **Tablet Mode**. Then run the program.

This program was adapted from a similar program described in the [September 2012 issue of _MSDN Magazine_](https://msdn.microsoft.com/en-us/magazine/jj618305.aspx).

Author
------
Charles Petzold 







