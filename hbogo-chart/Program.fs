// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open HboGo
open Excel

[<EntryPoint>]
let main argv = 
    let fileName = "movies.xlsx"
    let sheetName = "Movies"
    Excel.createSpreadsheet fileName sheetName HboGo.getHboGoMovies
    
    0 // return an integer exit code
    
