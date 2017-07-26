// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open HboGo

[<EntryPoint>]
let main argv = 
    let movies = HboGo.getHboGoMovies
    printfn "%A" movies
    System.Console.ReadKey() |> ignore

    0 // return an integer exit code
