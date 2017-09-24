module Excel

open HboGo
open ClosedXML.Excel

let saveToExcel (filepath:string) (movies:HboMovieWithRating seq) =
  let wb = new XLWorkbook()
  let ws = wb.Worksheets.Add("Movies")
  ws.Cell(1, 1).InsertTable(movies) |> ignore
  ws.Cell("A1").Value <- "Movie"
  ws.Cell("B1").Value <- "HBO URL"
  ws.Cell("C1").Value <- "ČSFD URL"
  ws.Cell("D1").Value <- "ČSFD rating"
  ws.Columns().AdjustToContents() |> ignore
  wb.SaveAs(filepath)