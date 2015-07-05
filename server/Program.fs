open System
open System.IO

open Suave
open Suave.Logging
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Http.RequestErrors
open Suave.Web
open Suave.Types
open System.Net


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
            >>= GET >>= OK "Hello from F#";
        serveFiles;
        NOT_FOUND "Found no handlers"
    ]

[<EntryPoint>]
let main argv = 
    let port = 
        let p, a = UInt16.TryParse argv.[0]
        if p then a
        else 3000us
    let contentPath = Some (Path.Combine(Environment.CurrentDirectory, "public"))
    let config = { 
        defaultConfig with
            homeFolder = contentPath;
            bindings = [ HttpBinding.mk HTTP IPAddress.Loopback port];
            logger = Loggers.ConsoleWindowLogger (minLevel=LogLevel.Warn, colourise=true)
    }
    startWebServer config  app
    0
