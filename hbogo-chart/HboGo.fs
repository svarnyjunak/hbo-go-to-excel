module HboGo

open System
open HtmlAgilityPack
open Types

let getHboGoMovies =
    let url = "http://www.hbo.cz/page/movies"
    let html = HtmlDownloader.getHtmlDocument url
    let nodes = html.DocumentNode.SelectNodes "//div[@class=\"alphabet_item_container\"]/a"
    let nodeList = List.ofSeq nodes 

    let getHboMovieName (node:HtmlNode) =
        let text = node.InnerText
        text.Trim('\n', '\r', '\t', ' ')
    
    let getHboMovieUrl (node:HtmlNode) =
        let hboUrl = new Uri("http://www.hbo.cz")
        let relativePath = node.Attributes.["href"].Value
        let uri = new Uri(hboUrl, relativePath)
        uri.AbsoluteUri

    let createHboMovie (node:HtmlNode) =
        {Name=getHboMovieName node; HboUrl=getHboMovieUrl node}

    nodeList |> List.map createHboMovie