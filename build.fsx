// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open Fake

// Directories
let buildDir  = "./build/"
let deployDir = "./deploy/"
let packagedDir = "./packaged/"


// Filesets
let appReferences  =
    !! "/**/*.csproj"
      ++ "/**/*.fsproj"

// version info
let version = "0.1"  // or retrieve from CI server

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; deployDir]
)

Target "Build" (fun _ ->
    // compile all projects below src/app/
    MSBuildDebug buildDir "Build" appReferences
        |> Log "AppBuild-Output: "
)

Target "Deploy" (fun _ ->
    !! (buildDir + "/**/*.*")
        -- "*.zip"
        |> Zip buildDir (deployDir + "ApplicationName." + version + ".zip")
)

Target "CreatePackage" (fun _ ->
  //CopyFiles buildDir packagedDir

  NuGet (fun p ->
    {p with
      Authors = ["@peterchch"]
      Project = "Neo4j.TypeProvider"
      Description = "A type provider for providing the schema of a Neo4j database, and proxies for it's Nodes"
      OutputPath = packagedDir
      WorkingDir = "."
      Summary = "A type provider for providing the schema of a Neo4j database, and proxies for it's Nodes"
      Version = "0.2"
      Publish = false
      Files = [(@"build/neo4jtypeprovider.dll", Some @"lib/net45", None) ]
      Dependencies =
            ["MetaTp", GetPackageVersion "./packages/" "MetaTp"
             "Neo4jClient", GetPackageVersion "./packages/" "Neo4jClient"
             "Newtonsoft.Json", GetPackageVersion "./packages/" "Newtonsoft.Json" 
            ]
      }
    )
    "neo4jtypeprovider.nuspec"
)

// Build order
"Clean"
  ==> "Build"
  ==> "Deploy"

// start build
RunTargetOrDefault "Build"
