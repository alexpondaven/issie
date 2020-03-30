﻿module Renderer

open Fulma
open Elmish
open Elmish.HMR
open Elmish.React
open Elmish.Debug
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Core
open Fable.Core.JsInterop

open Types
open MyStyle
open DrawLib

importSideEffects "./../../app/scss/main.scss" 

// TODO move all of these JS functions and interface them properly.

[<Emit("typeof $0")>]
let jsType (x: obj) : unit = jsNative

[<Emit("console.log($0)")>]
let log msg : unit = jsNative

[<Emit("alert($0)")>]
let alert msg : unit = jsNative

type Page =
    | DiagramPage

type Model = {
    Page : Page
    Diagram : Diagram.Model
}

type Messages =
    | PageMsg of Page
    | DiagramMsg of Diagram.Messages

// -- Init Model

let init() = {
    Page = DiagramPage
    Diagram = Diagram.init()
}

// -- Create View

let pageView model dispatch =
    match model.Page with
    | DiagramPage -> Diagram.view model.Diagram (DiagramMsg >> dispatch)

let view model dispatch =
    div [] [
        //Navbar.navbar [] [
        //    Navbar.Item.div [ Navbar.Item.IsHoverable ] [
        //        Navbar.Link.div [ Navbar.Link.IsArrowless; Navbar.Link.Option.Props.OnClick (fun _ -> PageMsg DiagramPage |> dispatch) ] [ str "Diagram" ]
        //    ]
        //]
        Button.button [Button.OnClick (fun _ -> PageMsg DiagramPage |> dispatch )] [str "Diagram"]
        Button.button [Button.OnClick (fun _ -> PageMsg DiagramPage |> dispatch )] [str "Editor"]
        pageView model dispatch
    ]

// -- Update Model

let update msg model =
    match msg with
    | PageMsg page -> { model with Page = page } 
    | DiagramMsg msg' -> { model with Diagram = Diagram.update msg' model.Diagram }

Program.mkSimple init update view
|> Program.withReact "electron-app"
|> Program.run