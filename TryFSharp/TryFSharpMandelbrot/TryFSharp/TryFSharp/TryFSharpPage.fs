namespace TryFSharp

open System
open System.Numerics
open Xamarin.Forms
open Xamarin.Forms.Xaml
open Demo.Numerics

type TryFSharpPage() = 
    inherit ContentPage()

    let _ = base.LoadFromXaml(typeof<TryFSharpPage>)
    let image = base.FindByName<Image>("image")

    let rows = 1000
    let cols = 1000
    let center = Complex(-0.75, 0.0)
    let width = 2.5
    let height = 2.5
    let ColToX col = center.Real - width / 2.0 + float(col) * width / float(cols)
    let RowToY row = center.Imaginary - height / 2.0 + float(row) * height / float(rows)

    let IsMandelbrot c iterations = 
        let mutable z = new Complex()
        let mutable rep = 0
        while rep < iterations && z.Magnitude < 2.0 do
            z <- z * z + c
            rep <- rep + 1
        rep = iterations

    member this.OnCalculateButtonClicked(sender : Object, args : EventArgs) =
        let button = sender :?> Button
        do button.IsEnabled <- false
         
        let stream = Bmp.Create rows cols (fun row col ->
            let x = ColToX col
            let y = RowToY row
            let c = Complex(x, y)
            if IsMandelbrot c 100 then Color.Black else Color.White
            )
        do image.Source <- ImageSource.FromStream(fun _ -> stream)
        do button.IsEnabled <- true

type App() = 
    inherit Application(MainPage = TryFSharpPage())


