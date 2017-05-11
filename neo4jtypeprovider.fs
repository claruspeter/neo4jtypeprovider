module Haumohio.Neo4j.core
open Microsoft.FSharp.Core.CompilerServices
open metatp
open Haumohio.Neo4j.graph

let private paras = 
  [
    {name="ConnectionString"; coltype=typeof<string>}
    {name="User"; coltype=typeof<string>}
    {name="Pwd"; coltype=typeof<string>}
  ]

let private buildProvider connectionstring user pwd =
    let neo = Haumohio.Neo4j.graph.Connect connectionstring user pwd
    let relTables =
      neo.relList
      |> List.map (fun nm -> {name=nm; columns=[||]} )
    let nodeTables =
      neo.labelList
      |> List.map (fun nm -> {name=nm; columns = neo.propList nm |> List.map (fun p -> {name=p; coltype = typeof<string>}) |> List.toArray })
    nodeTables @ relTables |> List.toArray


let private loaddata (parameterValues:obj[]) =
  match parameterValues with
  | [|:? string as connectionstring|] ->
    buildProvider connectionstring "neo4j" "neo4j"
  | [|:? string as connectionstring; :? string as user; :? string as pwd;|] ->
    buildProvider connectionstring user pwd
  | _ -> failwith "connection string not specified"

[<Microsoft.FSharp.Core.CompilerServices.TypeProvider>]
type Neo4jTypeProvider(config) =
  inherit metatp.MetaProvider(config, "Haumohio.Neo4j","Schema", paras, loaddata )


[<assembly:Microsoft.FSharp.Core.CompilerServices.TypeProviderAssembly>]
do ()
