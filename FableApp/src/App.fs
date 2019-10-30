module FableApp

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props

type Model = int

type Msg =
    | Increment
    | Decrement

let init() : Model = 0

let update (msg : Msg) (model : Model) =
    match msg with
    | Increment -> model + 1
    | Decrement -> model - 1

// Rendered with Preact
let view (model : Model) dispatch =
    div []
      [ button [ OnClick (fun _ -> dispatch Increment) ] [ str "+" ]
        div [] [ str (string model) ]
        button [ OnClick (fun _ -> dispatch Decrement) ] [ str "-" ] ]

Program.mkSimple init update view
|> Program.withReactSynchronous "elmish-app"
|> Program.withConsoleTrace
|> Program.run