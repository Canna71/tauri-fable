module App

open Fable.Core
open Fable.React
open Fable.Core.JsInterop
open FreeAct
open FreeFrame
open Util
// open reactLogo from "./assets/react.svg";
// open fableLogo from "./assets/fable.svg";

let [<Import("default", from="./assets/react.svg")>] reactLogo: string = jsNative
let [<Import("default", from="./assets/fable.svg")>] fableLogo: string = jsNative


type AppState = {
    Name: string
    Message: string
}

// Initialize the app database with initial state
let appDb = AppDb<AppState>({
    Name = ""
    Message = ""
})

let setNameEvent = EventId.named<string> "set-name"

let setNameEventHandler = registerEventHandler setNameEvent (fun name state ->
    { state with Name = name }
)

let nameSubscription = createView appDb (fun state ->
    state.Name
)
let messageSubscription = createView appDb (fun state ->
    state.Message
)

let setMessageEvent = EventId.named<string> "set-message"

let setMessageEventHandler = registerEventHandler setMessageEvent (fun message state ->
    { state with Message = message }
)

let fetchMessageEffect = EffectId.named<string, unit> "fetch-message"

Effects.registerHandler fetchMessageEffect (fun name ->
    async {
        let! (greetMsg : string) =
            Tauri.invoke(
                "greet",
                createObj [
                    "name" ==> name
                ])

        dispatch appDb setMessageEvent greetMsg
        // return message
    }
)


[<JSX.Component>]
let AppComponent () : ReactElement =
    let name = useView nameSubscription
    let greetMsg = useView messageSubscription

    /// Learn more about Tauri commands at https://tauri.app/v1/guides/features/command
    // let greet () : unit =
    //     async {
    //         let! (greetMsg : string) =
    //             Import.Tauri.invoke (
    //                 "greet",
    //                 createObj [
    //                     "name" ==> name
    //                 ]
    //             ) |> Async.AwaitPromise
    //         setGreetMsg greetMsg
    //     }
    //     |> Async.StartImmediate


    let onSubmit' (e : Browser.Types.Event) : unit =
        e.preventDefault()
        Effects.runEffect fetchMessageEffect name
        |> ignore
        // greet()

    let onChange' (e : Browser.Types.Event) : unit =
        let value : string =
            !!e.target?value
        dispatch appDb setNameEvent value

    div {
        className "container"
        h1 { "Welcome to Tauri! "}
        div {
            className "row"
            a {
                href "https://vitejs.dev" ; target Target.Blank
                img {
                    src "./vite.svg" ; className "logo vite" ; alt "Vite Logo"
                }
            }
            a {
                href "https://tauri.app"; target Target.Blank
                img {
                    src "./tauri.svg"; className "logo tauri"; alt "Tauri Logo"
                }
            }
            a {
                href "https://reactjs.org"; target Target.Blank
                img {
                    src reactLogo; className "logo react"; alt "React Logo"
                }
            }
            a {
                href "https://fable.io"; target Target.Blank
                img {
                    src fableLogo; className "logo react"; alt "Fable Logo"
                }
            }

        }
        p { "Click on the Tauri, Vite, React, and Fable logos to learn more." }
        form {
            className "row"; onSubmit onSubmit'
            input {
                id "greet-input"; onChange onChange'; placeholder "Enter a name..."
            }
            button {
                type' InputType.Submit
                "Greet"
            }
        }
        p { greetMsg }
    }


