// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open HboGo
open System.Diagnostics

[<EntryPoint>]
let main argv = 
    printfn "Hbo movies download started"

    let fileName = "movies.xlsx"
    let movies = List.sortByDescending (fun m-> m.CsfdRating) HboGo.getHboGoMovies
    Excel.saveToExcel fileName movies
    Process.Start fileName |> ignore

    printfn "Hbo movies download finished"

    0 // return an integer exit code
    
