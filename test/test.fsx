#r "../node_modules/fable-core/Fable.Core.dll"

#load "../src/wrap/util.fs"
#load "../src/wrap/serialize.fs"
#load "../src/wrap/buffer.fs"
#load "../src/wrap/shortid.fs"
#load "../src/wrap/q.fs"
#load "../src/wrap/q-io.fs"
#load "../src/wrap/dns.fs"
#load "../src/wrap/ipaddr.fs"
#load "../src/wrap/network.fs"
#load "../src/wrap/bonjour.fs"
#load "../src/wrap/crypto.fs"
#load "./mocha.fs"

open Util
open Buffer
open MochaTest

type DoneF = unit -> unit
type It = string * (DoneF -> unit)

let (=>) (a : string) (b : 'b) : (string * 'b) = (a,b)

let final f _ = f ()

let tests : (string * (It list)) list =
  [ "smoke" => 
      [ "should not come out" =>
          fun donef ->
            begin
              massert.ok true ;
              donef ()
            end
      ]
  ]

let _ =
  List.map
    (fun (n,t) ->
      describe
        n
        (fun () ->
          List.map (fun (itName,itTest) -> it itName itTest) t
          |> ignore
        )
    )
    tests
