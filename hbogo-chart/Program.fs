// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open HboGo
open System.Diagnostics

[<EntryPoint>]
let main argv = 
    let fileName = "movies.xlsx"
    let sheetName = "Movies"
    let movies = List.sortByDescending (fun m-> m.CsfdRating) HboGo.getHboGoMovies
    Excel.createSpreadsheet fileName sheetName movies
    Process.Start fileName |> ignore

    0 // return an integer exit code
    
