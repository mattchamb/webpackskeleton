// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------

#r @"src/fs/packages/FAKE/tools/FakeLib.dll"

open Fake
open System.Diagnostics

type BuildConfig = {outputDir: string; webpackConfig: string; releaseBuild: bool}

let serverProj = !!"./src/fs/ApiServer/ApiServer.fsproj"
let nodePath = environVarOrDefault "NodePath" "node"
let npmPath = environVarOrDefault "NpmPath" "npm"

let exec proc wd args =
    let webpackArgs (pi: ProcessStartInfo) =
        pi.FileName <- proc
        pi.WorkingDirectory <- wd
        pi.Arguments <- args
    directExec webpackArgs

let buildCfg =
    let releaseBuild = getEnvironmentVarAsBoolOrDefault "ReleaseBuild" false
    if releaseBuild then
        { outputDir = "./bin/Release"
          webpackConfig = "webpack.prod.config.js"
          releaseBuild = releaseBuild }
    else 
        { outputDir = "./bin/Debug"
          webpackConfig = "webpack.config.js"
          releaseBuild = releaseBuild }

Target "Clean" (fun _ -> 
    CleanDirs [buildCfg.outputDir]
)

Target "BuildSiteAssets" (fun _ ->
    let args = sprintf "node_modules/webpack/bin/webpack.js --config %s" buildCfg.webpackConfig
    if not <| exec nodePath "./src/js/" args then
        failwith "Failed to bundle the site with webpack."
)

Target "BuildApiServer" (fun _ ->
    if buildCfg.releaseBuild then
        MSBuildReleaseExt buildCfg.outputDir [("AzureFakeOverride", "true")] "Build" serverProj
            |> Log "AppBuild-Output: "
    else
        MSBuildDebug buildCfg.outputDir "Build" serverProj
            |> Log "AppBuild-Output: "
)

Target "BuildPrerenderer" (fun _ -> 
    ()
)

Target "All" DoNothing

"Clean" ==> "BuildSiteAssets"
"Clean" ==> "BuildApiServer"
"Clean" ==> "BuildPrerenderer"
"BuildSiteAssets" ==> "All"
"BuildPrerenderer" ==> "All"
"BuildApiServer" ==> "All"

RunTargetOrDefault "All"
