# Neo4jTypeProvider


This is an F# type provider for **Neo4j**.  It provides access and type accessibility
to *Labels* and *Relationship Types*, as well as the *Property Keys* of
the nodes.

This is useful when using the **Cypher** fluent api to query and reference
entities that exist in a Neo4j database, without the need for matching code proxies.

### Nuget

[Install-Package Neo4jTypeProvider](https://nuget.org/packages/Neo4jTypeProvider)

### Code Examples
```fsharp
    //#r "neo4jtypeprovider.dll"
    //#r "Neo4jClient.dll"

    open System
    open Haumohio.Neo4j
    open Neo4jClient

    [<Literal>]
    let connectionstring = @"http://localhost:7474/db/data"

    type schema = Haumohio.Neo4j.Schema<connectionstring>
    let db = new Neo4jClient.GraphClient(Uri(connectionstring))
    db.Connect()

    db.Cypher
      .Match("(p:" + schema.Person.NAME + ")")
      .Where( "p.born=1973" )
      .Return<schema.Proxies.Person>("p")
      .Limit(Nullable<int>(10))
      .Results
      |> Seq.toList
```
