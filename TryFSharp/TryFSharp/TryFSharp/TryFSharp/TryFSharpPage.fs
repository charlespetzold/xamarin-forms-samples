namespace TryFSharp

open System
open Xamarin.Forms
open Xamarin.Forms.Xaml
open Demo.Numerics

type TryFSharpPage() = 
    inherit ContentPage()

    let _ = base.LoadFromXaml(typeof<TryFSharpPage>)
    let image = base.FindByName<Image>("image")
    let elapsedLabel = base.FindByName<Label>("elapsedLabel")
    let progressBar = base.FindByName<ProgressBar>("progressBar")
    let zoomStepper = base.FindByName<Stepper>("zoomStepper")
    let iterStepper = base.FindByName<Stepper>("iterStepper")

    let rows = 1000
    let cols = 1000
    let center = Complex(-0.754674, -0.057078)
    let baseWidth = 2.5
    let baseHeight = 2.5
    let ColToX col width = center.Real - width / 2.0 + float(col) * width / float(cols)
    let RowToY row height = center.Imaginary - height / 2.0 + float(row) * height / float(rows)

    let MandelbrotIndex c iterations = 
        let rec RecursiveMandelbrot (z : Complex) countdown =
            if countdown = 0 then
                -1
            elif z.Magnitude >= 2.0 then
                iterations - countdown
            else
                RecursiveMandelbrot (z * z + c) (countdown - 1)
        RecursiveMandelbrot (Complex()) iterations

    let CalculateAsync(width, height, iterations) = 
        async {
            let stream = Bmp.Create rows cols (fun row col ->

                if (col = 0 && float row % float (rows / 100) = 0.0) then 
                    Device.BeginInvokeOnMainThread(fun _ ->
                        do progressBar.Progress <- float row / float rows)

                let x = ColToX col width
                let y = RowToY row height
                let c = Complex(x, y)

                let index = MandelbrotIndex c iterations
                match index with
                    | -1 -> Color.Black
                    | _ -> Color.FromHsla(float index / 64.0 % 1.0, 1.0, 0.5)
                )
            Device.BeginInvokeOnMainThread(fun _ ->
                do progressBar.Progress <- 1.0)

            return stream
        }

    member this.OnCalculateButtonClicked(sender : Object, args : EventArgs) =
        async {
            let button = sender :?> Button
            do button.IsEnabled <- false
            do elapsedLabel.Text <- " "
            let dtStart = DateTime.Now

            let zoom = 2.0 ** zoomStepper.Value
            let width = baseWidth / zoom
            let height = baseHeight / zoom
            let iterations = 1 <<< int iterStepper.Value

            let! token = Async.StartChild(CalculateAsync(width, height, iterations))
            let! stream = token
         
            do image.Source <- ImageSource.FromStream(fun _ -> stream)
            do elapsedLabel.Text <- (DateTime.Now - dtStart).ToString("mm\:ss\.f")
            do button.IsEnabled <- true
        } |> Async.StartImmediate

type App() = 
    inherit Application(MainPage = TryFSharpPage())


