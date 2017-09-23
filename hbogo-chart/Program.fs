// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open HboGo
open Excel
open System.Diagnostics

[<EntryPoint>]
let main argv = 
    let fileName = "movies.xlsx"
    let sheetName = "Movies"
    Excel.createSpreadsheet fileName sheetName HboGo.getHboGoMovies
    Process.Start fileName |> ignore

    0 // return an integer exit code
    
