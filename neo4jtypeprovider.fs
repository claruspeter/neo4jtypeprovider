module Haumohio.Neo4j.core
open Microsoft.FSharp.Core.CompilerServices
open metatp
open Haumohio.Neo4j.graph

let private loaddata (parameterValues:obj[]) =
  match parameterValues with
  | [|:? string as connectionstring|] ->
    //let connection = graph.Connect(connectionString)
    [|{name="Jack"; columns = [||]}|]
  | _ -> failwith "connection string not specified"

[<Microsoft.FSharp.Core.CompilerServices.TypeProvider>]
type Neo4jTypeProvider(config) =
  inherit metatp.MetaProvider(config, "Joe","Cool", [{name="ConnectionString"; coltype=typeof<string>}], loaddata )


[<assembly:Microsoft.FSharp.Core.CompilerServices.TypeProviderAssembly>]
do ()
