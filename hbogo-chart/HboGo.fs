module HboGo

open FSharp.Data
open HtmlAgilityPack
open System
open System.IO

let removeWhitespaces (s:string) =
    s.Trim('\n', '\r', '\t', ' ')

let getHtmlDocument url =
    let html = Http.RequestString url
    let htmlDocument = HtmlDocument()
    htmlDocument.LoadHtml html
    htmlDocument

type HboMovie = {Name:string; HboUrl:string}
type HboMovieWithRating = {Name:string; HboUrl:string; CsfdRating:string}


let getHboGoMovies =
    let url = "http://www.hbo.cz/page/movies"
    let html = getHtmlDocument url
    let nodes = html.DocumentNode.SelectNodes "//div[@class=\"alphabet_item_container\"]/a"
    let nodeList = List.ofSeq nodes

    let getHboMovieName (node:HtmlNode) =
        let text = node.InnerText
        removeWhitespaces text
    
    let getHboMovieUrl (node:HtmlNode) =
        let hboUrl = new Uri("http://www.hbo.cz")
        let relativePath = node.Attributes.["href"].Value
        let uri = new Uri(hboUrl, relativePath)
        uri.AbsoluteUri
    
    let getCsfdMovieUrl (name:string) =
        let csfdSearchUrl = "http://www.csfd.cz/hledat/?q=" + name
        let csfdSearchHtml = getHtmlDocument csfdSearchUrl
        let firstResult = csfdSearchHtml.DocumentNode.SelectSingleNode "//div[@id=\"search-films\"]/*/ul/li/a"
        
        match firstResult with
            | node when node = null -> ""
            | node -> let movieRelativePath = firstResult.Attributes.["href"].Value
                      let csfdUri = new Uri("http://www.csfd.cz/")
                      let uri = new Uri(csfdUri, movieRelativePath)
                      uri.AbsoluteUri

    let getCsfdRating url =
        let csfdMovieHtml = getHtmlDocument url
        let ratingNode = csfdMovieHtml.DocumentNode.SelectSingleNode "//div[@id=\"rating\"]/h2[@class=\"average\"]"
        match ratingNode with
            | node when node = null -> ""
            | node -> node.InnerText

    let createHboMovie (node:HtmlNode) =
        {Name=getHboMovieName node; HboUrl=getHboMovieUrl node}
    
    let createHboMovieWithRating (movie: HboMovie) =
        let csfdMovieUrl = getCsfdMovieUrl movie.Name
        printfn "%A" movie
        match csfdMovieUrl with
            | url when url = "" -> {Name=movie.Name; HboUrl=movie.HboUrl; CsfdRating=""}
            | url -> let rating = getCsfdRating csfdMovieUrl
                     {Name=movie.Name; HboUrl=movie.HboUrl; CsfdRating=rating}

    let moviesWithRating = nodeList |> List.map createHboMovie
                                    |> List.map createHboMovieWithRating
    
    let wr = new System.IO.StreamWriter(new FileStream("csv.csv", FileMode.OpenOrCreate), new Text.UnicodeEncoding())
    moviesWithRating |> List.map(fun m -> m.Name + ";" + m.HboUrl + ";" + m.CsfdRating) |> String.concat(System.Environment.NewLine) |> wr.Write
    wr.Close()

    moviesWithRating
    