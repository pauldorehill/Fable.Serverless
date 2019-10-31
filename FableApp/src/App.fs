module FableApp

open System
open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fable.Core
open Fable.Import
open Thoth
open Thoth.Fetch
open SharedDomain

// Currently need to set the URL manually for local testing etc
type Model =
    { User : User
      Info : string }
    member this.UpdateUser user = { this with User = user }

type Msg =
    | Increment
    | Decrement
    | UpdateName of string
    | Submit
    | PostedJson of User

// Used to test where its running
// So can deploy locally and to azure
let rootUrl = string Browser.Dom.window.location

let init () : Model * Cmd<Msg> =
    let user = { Name = "" ; Count = 0; Message = "" }
    { User = user; Info = "" }, Cmd.none

let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
    match msg with
    | Increment ->  model.UpdateUser { model.User with Count = model.User.Count + 1 }, Cmd.none
    | Decrement -> model.UpdateUser { model.User with Count = model.User.Count - 1 }, Cmd.none
    | UpdateName newName -> model.UpdateUser { model.User with Name = newName }, Cmd.none
    | Submit ->
        let msg : JS.Promise<Msg> =
            promise {
                let! post = Fetch.post(rootUrl + "json", model.User)
                return (PostedJson post)
            }
        let newUser = if model.User.Message <> "" then { model.User with Message = "Trying again..." } else model.User
        let newInfo =
            Math.Abs model.User.Count
            |> sprintf "Sent to server. Please wait %is and continue clicking to show non blocking UI"
        let newModel = { model with Info = newInfo; User = newUser }
        newModel, Cmd.OfPromise.result msg
    | PostedJson user -> { model with Info = "The count is now at your original value"; User = user } , Cmd.none

// Rendered with Preact. See package.json
let view (model : Model) dispatch =
  div []
      [ div []
            [ str "Name"; input [ Value model.User.Name;  OnChange (fun s -> dispatch (UpdateName s.Value)) ] ]
        div []
            [ button [ OnClick (fun _ -> dispatch Increment) ] [ str "+" ]
              div [] [ str (string model.User.Count) ]
              button [ OnClick (fun _ -> dispatch Decrement) ] [ str "-" ] ]

        button [ OnClick (fun _ -> dispatch Submit) ] [ str "Send" ]
        p [] [ str model.User.Message ]
        p [] [ str model.Info ] ]

Program.mkProgram init update view
|> Program.withReactSynchronous "elmish-app"
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.run
