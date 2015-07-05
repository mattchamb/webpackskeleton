// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------

#r @"packages/FAKE/tools/FakeLib.dll"

open Fake
open System.Diagnostics

let buildDir = "./build/Release"
let serverProj = !! "server/Server.fsproj"
let nodePath = environVarOrDefault "NodePath" "node"

Target "Clean" (fun _ -> 
    CleanDirs [buildDir]
)

Target "BuildSite" (fun _ ->
    let webpackArgs (pi: ProcessStartInfo) =
        pi.FileName <- nodePath
        pi.Arguments <- "node_modules/webpack/bin/webpack.js --config webpack.prod.config.js"
    if not <| directExec webpackArgs then
        failwith "Web"
)

Target "BuildServer" (fun _ ->
    MSBuildReleaseExt buildDir [("AzureFakeOverride", "true")] "Build" serverProj
        |> Log "AppBuild-Output: "
)

// --------------------------------------------------------------------------------------
// Run all targets by default. Invoke 'build <Target>' to override

Target "All" DoNothing

"Clean"
    ==> "BuildServer"
    ==> "BuildSite"

RunTargetOrDefault "BuildSite"

