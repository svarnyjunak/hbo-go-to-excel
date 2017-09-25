module HtmlDownloader

open System
open FSharp.Data
open HtmlAgilityPack

let getHtmlDocument url : HtmlDocument =
  let html = Http.RequestString url
  let htmlDocument = HtmlDocument()
  htmlDocument.LoadHtml html
  htmlDocument
