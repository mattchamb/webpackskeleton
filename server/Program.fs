open System
open System.IO

open Suave
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Http.RequestErrors
open Suave.Web
open Suave.Types


let serveFiles = 
    GET >>= pathRegex ".*"
        >>= choose [
            // Try serving the requested file
            request(fun r -> Files.browseFileHome r.url.LocalPath);
            // Otherwise serve index.html so that the server works with html5 history api
            request(fun r -> Files.browseFileHome "index.html");
        ]

let app = 
    choose [
        path "/api/hello" 
            >>= choose [ GET >>= OK "Hello from F#"
                         NOT_FOUND "Found no handlers" ]
        serveFiles
        NOT_FOUND "Found no handlers"
    ]

[<EntryPoint>]
let main argv = 
    let contentPath = Some (Path.Combine(Environment.CurrentDirectory, "public"))
    startWebServer {defaultConfig with homeFolder = contentPath} app
    0
