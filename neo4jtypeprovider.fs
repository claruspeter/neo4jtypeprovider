module Neo4j.TypeProvider.Core
open Microsoft.FSharp.Core.CompilerServices
open MetaTp
open Neo4j.TypeProvider.Graph

let private paras = 
  [
    {name="ConnectionString"; paratype=typeof<string>}
    {name="User"; paratype=typeof<string>}
    {name="Pwd"; paratype=typeof<string>}
  ]

let private buildProvider connectionstring user pwd =
    let neo = Neo4j.TypeProvider.Graph.Connect connectionstring user pwd
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
  inherit MetaTp.MetaProvider(config, { nameSpace = "Neo4j.TypeProvider"; typeName = "Schema"; yourTypeParameters = paras; schemaFromParameters = loaddata } )


