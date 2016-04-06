// Learn more about F# at http://fsharp.net. See the 'F# Tutorial' project
// for more guidance on F# programming.

#I "build"
#r "neo4jtypeprovider.dll"
open Haumohio.Neo4j


// Define your library scripting code here
type test = Joe.Cool<"a">
let qqq = test.Proxies.Jack()
qqq
