namespace TryFSharp

open System
open Xamarin.Forms

type TryFSharpPage() = 
    inherit ContentPage()

    do base.Content <- Label(Text = "Hello, F# Xamarin.Forms",
                             HorizontalOptions = LayoutOptions.Center,
                             VerticalOptions = LayoutOptions.Center,
                             FontSize = 24.0,
                             TextColor = Color.Pink,
                             FontAttributes = FontAttributes.Italic,
                             Rotation = -15.0)

type App() = 
    inherit Application(MainPage = TryFSharpPage())


