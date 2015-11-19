TryFSharp
=========

F# Xamarin.Forms samples described in the blog entry [*Writing Xamarin.Forms Apps in F#*](http://www.charlespetzold.com/blog/2015/10/Writing-Xamarin-Forms-Apps-in-FSharp.html), first posted November 4, 2015.

This blog entry and samples originated with talks at [F# Gotham](http://www.fsharpgotham.com/) on October 17, 2015 and [NYC .NET Mobile Developers](http://www.meetup.com/nycmobiledev/events/225356544/) on October 27, 2015.

The Windows Phone 8.1 project runs successfully only on an emulator; 
when deployed to a device it raises an exception at the `Forms.Init` related to its inability to load the FSharp.Core.dll library.
This problem cannot be fixed by adding a reference to FSharp.Core.dll, 
either through referencing the file in the *Reference Assembles* directory, or by installing the *FSharp.Core* NuGet package.
