KwazyThreeDee
=============

KwazyThreeDee (or "quasi-3D") was seen in the Xamarin Evolve 2014 talk
[Xamarin.Forms is Even Cooler than You Think](https://www.youtube.com/watch?v=79SdhVjBwh0). 
It demonstrates the use of *BoxView* to mimic vector graphics lines
in Xamarin.Forms. These lines are arranged and rotated in space using some 3D math. This
technique mostly works until a sphere is attempted, and then it brings Xamarin.Forms to its knees.

Some of the code and concepts of the **Forms3D** library are drawn from the book *3D Programming
for Windows* by Charles Petzold (Microsoft Press, 2007).

The program runs on iPhone, Android, and Windows Phone.
On Android, an excessive amount of garbage collection occurs; this impacts the 
performance, particularly when running under a debugger.

The program is not currently working on the iOS Simulator for inexplicable reasons.

Author
------

Charles Petzold
