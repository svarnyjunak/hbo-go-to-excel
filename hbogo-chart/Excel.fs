module Excel

open DocumentFormat.OpenXml
open DocumentFormat.OpenXml.Packaging
open DocumentFormat.OpenXml.Spreadsheet
open HboGo


let createSpreadsheet (filepath:string) (sheetName:string) (movies:HboMovieWithRating list) =
    // Create a spreadsheet document by supplying the filepath.
    // By default, AutoSave = true, Editable = true, and Type = xlsx.
    
    using (SpreadsheetDocument.Create(filepath, SpreadsheetDocumentType.Workbook)) (fun spreadsheetDocument ->
    
        // Add a WorkbookPart to the document.
        let workbookpart = spreadsheetDocument.AddWorkbookPart()
        workbookpart.Workbook <- new Workbook()
    
        // Add a WorksheetPart to the WorkbookPart.
        // http://stackoverflow.com/questions/5702939/unable-to-append-a-sheet-using-openxml-with-f-fsharp
        let worksheetPart = workbookpart.AddNewPart<WorksheetPart>()
        worksheetPart.Worksheet <- new Worksheet(new SheetData():> OpenXmlElement)
    
        // Add Sheets to the Workbook.
        let sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets())
    
        // Append a new worksheet and associate it with the workbook.
        let sheet = new Sheet()
        sheet.Id <-  StringValue(spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart))
        sheet.SheetId <-  UInt32Value(1u) 
        sheet.Name <-  StringValue(sheetName)
        sheets.Append([sheet :> OpenXmlElement])

        let sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>()
        let headerRow = sheetData.AppendChild(new Row())

        let headerName = headerRow.AppendChild(new Cell())
        headerName.CellValue <- new CellValue("Movie")
        headerName.DataType <- new EnumValue<CellValues>(CellValues.String)

        let headerHboUrl = headerRow.AppendChild(new Cell())
        headerHboUrl.CellValue <- new CellValue("Hbo URL")
        headerHboUrl.DataType <- new EnumValue<CellValues>(CellValues.String)

        let headerCsfdRating = headerRow.AppendChild(new Cell())
        headerCsfdRating.CellValue <- new CellValue("ČSFD")
        headerCsfdRating.DataType <- new EnumValue<CellValues>(CellValues.String)

        let writeMovie (movie:HboMovieWithRating) =
            let row = sheetData.AppendChild(new Row())
            let cellName = row.AppendChild(new Cell())
            cellName.CellValue <- new CellValue(movie.Name)
            cellName.DataType <- new EnumValue<CellValues>(CellValues.String)

            let cellHboUrl = row.AppendChild(new Cell())
            cellHboUrl.CellValue <- new CellValue(movie.HboUrl)
            cellHboUrl.DataType <- new EnumValue<CellValues>(CellValues.String)

            let cellCsfdRating = row.AppendChild(new Cell())
            cellCsfdRating.DataType <- new EnumValue<CellValues>(CellValues.String)
            cellCsfdRating.CellValue <- new CellValue(movie.CsfdRating)


        movies |>  Seq.iter(fun m -> writeMovie m)

        workbookpart.Workbook.Save();
    )