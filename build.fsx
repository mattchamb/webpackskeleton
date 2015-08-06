// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------

#r @"src/fs/packages/FAKE/tools/FakeLib.dll"

open Fake
open System.Diagnostics

type BuildConfig = {outputDir: string; webpackConfig: string; releaseBuild: bool}

let serverProj = !!"./src/fs/ApiServer/ApiServer.fsproj"
let jsDir = "./src/js"
let nodePath = environVarOrDefault "NodePath" "node"
let npmPath = environVarOrDefault "NpmPath" @"c:\Program Files\nodejs\npm.cmd"

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
    CleanDir buildCfg.outputDir
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

let prerendererDir = buildCfg.outputDir @@ "prerenderer/"

Target "InstallNpmPackages" (fun _ -> 
    checkFileExists npmPath
    if not <| exec npmPath prerendererDir "install --production" then
        failwith "Failed to install prerenderer NPM packages"
)

Target "GulpBuild" (fun _ -> 
    checkFileExists npmPath
    if not <| exec nodePath jsDir "node_modules/gulp/bin/gulp.js" then
        failwith "Gulp build failed"
)

Target "CopyPrerendererPackageJson" (fun _ -> 
    let src = jsDir @@ "package.json"
    ensureDirectory prerendererDir
    CopyFile prerendererDir src
)

Target "CleanPrerenderer" (fun _ ->
    CleanDir prerendererDir
)

Target "BuildPrerenderer" DoNothing
Target "RebuildPrerenderer" DoNothing

"CleanPrerenderer"
    ==> "CopyPrerendererPackageJson"
    ==> "InstallNpmPackages"
    ==> "GulpBuild"
    ==> "RebuildPrerenderer"

"CopyPrerendererPackageJson"
    ==> "InstallNpmPackages"
    ==> "GulpBuild"
    ==> "BuildPrerenderer"

"CleanPrerenderer" ==> "Clean"

Target "All" DoNothing

"Clean" ==> "BuildSiteAssets"
"Clean" ==> "BuildApiServer"
"BuildSiteAssets" ==> "All"
"RebuildPrerenderer" ==> "All"
"BuildApiServer" ==> "All"

RunTargetOrDefault "All"
