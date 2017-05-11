// Learn more about F# at http://fsharp.net. See the 'F# Tutorial' project
// for more guidance on F# programming.

#I "build"
#r "neo4jtypeprovider.dll"
#r "Neo4jClient.dll"

open System
open Haumohio.Neo4j
open Neo4jClient

[<Literal>]
let connectionstring = @"http://localhost:7474/db/data"
[<Literal>]
let u = @"neo4j"
[<Literal>]
let p = @"neo4j"

type schema = Haumohio.Neo4j.Schema<ConnectionString=connectionstring, User=u, Pwd=p>
let db = new Neo4jClient.GraphClient(Uri(connectionstring), u, p)
db.Connect()

let aaa = 
  db.Cypher
    .Match("(p:" + schema.Person.NAME + ")")
    .Where( "p.born<>1973" )
    .Return<schema.Proxies.Person>("p")
    .Limit(Nullable<int>(10))
    .Results
    |> Seq.toList
