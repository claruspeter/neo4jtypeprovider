module Haumohio.Neo4j.core
open Microsoft.FSharp.Core.CompilerServices
open metatp
open Haumohio.Neo4j.graph

let private loaddata (parameterValues:obj[]) =
  match parameterValues with
  | [|:? string as connectionstring|] ->
    let neo = Haumohio.Neo4j.graph.Connect connectionstring
    let relTables =
      neo.relList
      |> List.map (fun nm -> {name=nm; columns=[||]} )
    let nodeTables =
      neo.labelList
      |> List.map (fun nm -> {name=nm; columns = neo.propList nm |> List.map (fun p -> {name=p; coltype = typeof<string>}) |> List.toArray })

    nodeTables @ relTables |> List.toArray
  | _ -> failwith "connection string not specified"

[<Microsoft.FSharp.Core.CompilerServices.TypeProvider>]
type Neo4jTypeProvider(config) =
  inherit metatp.MetaProvider(config, "Haumohio.Neo4j","Schema", [{name="ConnectionString"; coltype=typeof<string>}], loaddata )


[<assembly:Microsoft.FSharp.Core.CompilerServices.TypeProviderAssembly>]
do ()
