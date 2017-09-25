// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open System.Diagnostics
open Csfd
open HboGo
open Types

[<EntryPoint>]
let main argv = 
    printfn "Hbo movies download started"

    let printMovie (movie: HboMovie) =
        let previousColor = Console.ForegroundColor
        Console.ForegroundColor <- ConsoleColor.DarkRed
        printfn "%A" movie.Name
        Console.ForegroundColor <- previousColor    

    let createHboMovieWithRating (movie: HboMovie) =
        printMovie movie
        let csfdMovieUrl = getCsfdMovieUrl movie.Name
        match csfdMovieUrl with
            | url when url = "" -> {Name=movie.Name; HboUrl=movie.HboUrl; CsfdUrl=url; CsfdRating=""}
            | url ->               {Name=movie.Name; HboUrl=movie.HboUrl; CsfdUrl=url; CsfdRating=getCsfdRating csfdMovieUrl}
    
    let moviesWithRating = getHboGoMovies |> List.map createHboMovieWithRating
                                          |> List.sortByDescending (fun m-> m.CsfdRating)
    
    let fileName = "movies.xlsx"
    Excel.saveToExcel fileName moviesWithRating
    Process.Start fileName |> ignore

    printfn "Hbo movies download finished"

    0 // return an integer exit code
    
