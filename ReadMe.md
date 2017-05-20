# Neo4jTypeProvider


This is an F# type provider for **Neo4j**.  It provides access and type accessibility
to *Labels* and *Relationship Types*, as well as the *Property Keys* of
the nodes.

This is useful when using the **Cypher** fluent api to query and reference
entities that exist in a Neo4j database, without the need for matching code proxies.

### Nuget

[Install-Package Neo4j.TypeProvider](https://nuget.org/packages/Neo4j.TypeProvider)

### Code Examples
```fsharp
    //#r "neo4jtypeprovider.dll"
    //#r "Neo4jClient.dll"

    [<Literal>]
    let connectionstring = @"http://localhost:7474/db/data"
    [<Literal>]
    let user = @"neo4j"
    [<Literal>]
    let pwd = @"password"

    type schema = Neo4j.TypeProvider.Schema<ConnectionString=connectionstring, User=user, Pwd=pwd>
    let db = new Neo4jClient.GraphClient(Uri(connectionstring), user, pwd)
    db.Connect()

    db.Cypher
        .Match("(p:" + schema.Person.NAME + ")")
        .Where( "p.born<>1973" )
        .Return<schema.Person.Proxy>("p")
        .Limit(Nullable<int>(10))
        .Results
        |> Seq.toList

```
