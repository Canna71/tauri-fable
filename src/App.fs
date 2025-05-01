module App

open Fable.Core
open Fable.React
open Fable.Core.JsInterop
open FreeAct
// open reactLogo from "./assets/react.svg";
// open fableLogo from "./assets/fable.svg";

let [<Import("default", from="./assets/react.svg")>] reactLogo: string = jsNative
let [<Import("default", from="./assets/fable.svg")>] fableLogo: string = jsNative

module Import =

    [<Erase>]
    type Tauri =
        [<Import("invoke", "@tauri-apps/api/core")>]
        static member invoke(cmd : string, ?invokeParams : obj) : JS.Promise<_> = jsNative

[<JSX.Component>]
let AppComponent () : ReactElement =
    let greetState = Hooks.useState ""
    let greetMsg = greetState.current
    let setGreetMsg : string->unit = greetState.update
    let nameState = Hooks.useState ""
    let setName : string->unit = nameState.update
    let name = nameState.current

    /// Learn more about Tauri commands at https://tauri.app/v1/guides/features/command
    let greet () : unit =
        async {
            let! (greetMsg : string) =
                Import.Tauri.invoke (
                    "greet",
                    createObj [
                        "name" ==> name
                    ]
                ) |> Async.AwaitPromise
            setGreetMsg greetMsg
        }
        |> Async.StartImmediate


    let onSubmit' (e : Browser.Types.Event) : unit =
        e.preventDefault()
        greet()

    let onChange' (e : Browser.Types.Event) : unit =
        let value : string =
            !!e.target?value
        setName value

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

    // JSX.jsx $"""

    // <div className="container">
    //   <h1>Welcome to Tauri!</h1>

    //   <div className="row">
    //     <a href="https://vitejs.dev" target="_blank">
    //       <img src="/vite.svg" className="logo vite" alt="Vite logo" />
    //     </a>
    //     <a href="https://tauri.app" target="_blank">
    //       <img src="/tauri.svg" className="logo tauri" alt="Tauri logo" />
    //     </a>
    //     <a href="https://reactjs.org" target="_blank">
    //       <img src={{reactLogo}} className="logo react" alt="React logo" />
    //     </a>
    //     <a href="https://fable.io" target="_blank">
    //       <img src={{fableLogo}} className="logo react" alt="Fable logo" />
    //     </a>
    //   </div>

    //   <p>Click on the Tauri, Vite, React, and Fable logos to learn more.</p>

    //   <form
    //     className="row"
    //     onSubmit={onSubmit}
    //   >
    //     <input
    //       id="greet-input"
    //       onChange={onChange}
    //       placeholder="Enter a name..."
    //     />
    //     <button type="submit">Greet</button>
    //   </form>

    //   <p>{greetMsg}</p>
    // </div>
    // """
