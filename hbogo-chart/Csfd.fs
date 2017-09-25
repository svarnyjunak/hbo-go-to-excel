module Csfd

open System
open HtmlAgilityPack

let getCsfdMovieUrl (name:string) =
  let csfdSearchUrl = "http://www.csfd.cz/hledat/?q=" + name
  let csfdSearchHtml = HtmlDownloader.getHtmlDocument csfdSearchUrl
  let firstResult = csfdSearchHtml.DocumentNode.SelectSingleNode "//div[@id=\"search-films\"]/*/ul/li/a"
        
  match firstResult with
    | node when node = null -> ""
    | node -> let movieRelativePath = firstResult.Attributes.["href"].Value
              let csfdUri = new Uri("http://www.csfd.cz/")
              let uri = new Uri(csfdUri, movieRelativePath)
              uri.AbsoluteUri

let getCsfdRating url =
  let csfdMovieHtml = HtmlDownloader.getHtmlDocument url
  let ratingNode = csfdMovieHtml.DocumentNode.SelectSingleNode "//div[@id=\"rating\"]/h2[@class=\"average\"]"
  match ratingNode with
    | node when node = null -> ""
    | node -> node.InnerText