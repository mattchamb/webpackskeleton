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
    {prerenderedContent: string;
    scriptLocations: string array}

let defaultModel = {prerenderedContent=""; scriptLocations=[|"bundle.js"|]}

let indexTemplate = razor<TemplateModel> "Index.cshtml"

let jsFunc<'TArg, 'TResult> code (arg: 'TArg) =
    async {
        let func = Edge.Func code
        let boxedArg = box arg
        let! result = Async.AwaitTask <| func.Invoke boxedArg
        return result :?> 'TResult
    }

let test: int -> Async<int> = jsFunc "return function(x, cb) { cb(null, x * 2); }"

let serveFiles = 
    GET >>= pathRegex ".*"
        >>= choose [
            // Try serving the requested file
            request(fun r -> Files.browseFile (Path.Combine(Environment.CurrentDirectory, "public")) r.url.LocalPath);
            // Otherwise serve index.html so that the server works with html5 history api
            request(fun r -> indexTemplate defaultModel);
        ]

let app = 
    choose [
        path "/api/hello" 
            >>= GET >>= (fun x -> async {
                let! r = test 5;
                return! OK (sprintf "The result is: %A" r) x
            });
        serveFiles;
        NOT_FOUND "Found no handlers"
    ]

[<EntryPoint>]
let main argv = 
    printfn "Command line arguments: %A" argv

    let port = 
        if argv.Length >= 1 then
            printfn "Using port \"%s\"" argv.[0]
            let p, a = UInt16.TryParse argv.[0]
            if p then a
            else
                printfn "Unable to parse port \"%s\", defaulting to port 3000"  argv.[0]
                3000us
        else 
            printfn "Defaulting to port 3000"
            3000us

    let absoluteContentPath = 
        if argv.Length >= 2 then
            Some argv.[1]
        else
            Some Environment.CurrentDirectory

    let config = { 
        defaultConfig with
            homeFolder = absoluteContentPath;
            bindings = [ HttpBinding.mk HTTP IPAddress.Loopback port];
            logger = Loggers.ConsoleWindowLogger (minLevel=LogLevel.Verbose, colourise=true)
    }
    startWebServer config  app
    0
