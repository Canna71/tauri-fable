module Main

// Entry point must be in a separate file
// for Vite Hot Reload to work

open Browser
open Fable.React
open Fable.Core.JsInterop

importSideEffects "./styles.css"


// Initialize the application
let container = Browser.Dom.document.getElementById "root"
if isNull container then
    Browser.Dom.console.error "No element with id 'root' found."
else
    let root = ReactDomClient.createRoot(container)
    console.log "Rendering App..."
    root.render(App.AppComponent() |> unbox)
