module Main

// Entry point must be in a separate file
// for Vite Hot Reload to work

open Browser
open Fable.React
open Fable.Core.JsInterop

importSideEffects "./styles.css"

ReactDomClient.createRoot(document.getElementById("root")).render(

        App.AppComponent() |> unbox

)
