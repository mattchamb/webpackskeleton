// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------

#r @"packages/FAKE/tools/FakeLib.dll"

open Fake

let buildDir = "./build/Release"
let serverProj = !! "server/Server.fsproj"

Target "Clean" (fun _ -> 
    CleanDirs [buildDir]
)

Target "BuildSite" (fun _ ->
    let webpackArgs (pi: System.Diagnostics.ProcessStartInfo) =
        pi.FileName <- "webpack.cmd"
        pi.Arguments <- "--config webpack.prod.config.js"
    if not <| directExec webpackArgs then
        failwith "Web"
)

Target "BuildServer" (fun _ ->
    MSBuildRelease buildDir "Build" serverProj
        |> Log "AppBuild-Output: "
)

// --------------------------------------------------------------------------------------
// Run all targets by default. Invoke 'build <Target>' to override

Target "All" DoNothing

"Clean"
    ==> "BuildServer"
    ==> "BuildSite"

RunTargetOrDefault "BuildSite"

