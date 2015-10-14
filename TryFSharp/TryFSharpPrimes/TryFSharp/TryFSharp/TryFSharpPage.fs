namespace TryFSharp

open System
open Xamarin.Forms
open Xamarin.Forms.Xaml

type TryFSharpPage() = 
    inherit ContentPage()

    let _ = base.LoadFromXaml(typeof<TryFSharpPage>)
    let entry = base.FindByName<Entry>("entry")
    let listView = base.FindByName<ListView>("listView")

    member this.OnCalculateButtonClicked(sender : Object, args : EventArgs) = 
        let n = Int32.Parse(entry.Text)
        let m = (n + 1) / 2 - 2
        do listView.ItemsSource <-
            Array.init(m * m) (fun index -> (index % m + 2) * (index / m + 2))
            |> Set.ofArray
            |> Set.difference (Set.ofArray([| 2..n |]))

type App() = 
    inherit Application(MainPage = TryFSharpPage())


