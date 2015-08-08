open System
open System.IO
open EdgeJs
open Suave
open Suave.Logging
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Http.RequestErrors
open Suave.Web
open Suave.Types
open System.Net
open Suave.Razor

type TemplateModel = 
    { prerenderedContent : string
      scripts : string array }

let contentLocation =
#if DEBUG 
    "http://localhost:9090/"
#else
    ""
#endif
    

let defaultModel = 
    { prerenderedContent = ""
      scripts = [| contentLocation + "commons.js"
                   contentLocation + "main.js" |]
    }

let indexTemplate = razor<TemplateModel> "Index.cshtml"

let jsFunc<'TArg, 'TResult> code (arg : 'TArg) = 
    async { 
        let func = Edge.Func code
        let boxedArg = box arg
        let! result = Async.AwaitTask <| func.Invoke boxedArg
        return result :?> 'TResult
    }

let test : string -> Async<string> = jsFunc @"
        var prerender = require('../prerenderer/server/PreRenderer');
        return function(x, cb) { 
            try {
                prerender(x, cb);
            } catch(ex) {
                cb(ex);
            }
        };
    "

let useEntryPoint (url : Uri) = 
    let path = url.LocalPath
    async {
        let! r = test path
        let model = {defaultModel with prerenderedContent = r}
        return (fun x -> indexTemplate model <| x)
    }

let serveFiles publicDir = 
    GET >>= pathRegex ".*" >>= choose [ // Try serving the requested file
                                        request (fun r -> Files.browseFile publicDir r.url.LocalPath)
                                        path "/favicon.ico" >>= NOT_FOUND "File not found."
                                        // Otherwise serve index.html so that the server works with html5 history api
                                        request (fun r -> Async.RunSynchronously (useEntryPoint r.url)) ]

let app dir = 
    choose [ 
        path "/api/hello" >>= GET >>= OK "Hello"
        serveFiles (Path.Combine(dir, "public")) 
        NOT_FOUND "Found no handlers" ]

[<EntryPoint>]
let main argv = 
    printfn "Command line arguments: %A" argv
    let port = 
        if argv.Length >= 1 then 
            printfn "Using port \"%s\"" argv.[0]
            let p, a = UInt16.TryParse argv.[0]
            if p then a
            else 
                printfn "Unable to parse port \"%s\", defaulting to port 3000" argv.[0]
                3000us
        else 
            printfn "Defaulting to port 3000"
            3000us
    
    let absoluteContentPath = 
        if argv.Length >= 2 then argv.[1]
        else Environment.CurrentDirectory
    
    let config = 
        { defaultConfig with homeFolder = Some absoluteContentPath
                             bindings = [ HttpBinding.mk HTTP IPAddress.Loopback port ]
                             logger = Loggers.ConsoleWindowLogger(minLevel = LogLevel.Verbose, colourise = true) }
    
    startWebServer config (app absoluteContentPath)
    0
