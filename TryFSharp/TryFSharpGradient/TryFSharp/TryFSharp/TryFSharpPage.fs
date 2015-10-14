namespace TryFSharp

open System
open Xamarin.Forms
open Xamarin.Forms.Xaml

type TryFSharpPage() = 
    inherit ContentPage()

    let _ = base.LoadFromXaml(typeof<TryFSharpPage>)
    let image = base.FindByName<Image>("image")

    let rows = 1000
    let cols = 1000

    member this.OnCalculateButtonClicked(sender : Object, args : EventArgs) = 
        let stream = Bmp.Create rows cols (fun row col ->
            let red = float row / float rows
            let blue = float col / float cols
            Color.FromRgb(red, 0.0, blue)
            )
        do image.Source <- ImageSource.FromStream(fun _ -> stream)

type App() = 
    inherit Application(MainPage = TryFSharpPage())


