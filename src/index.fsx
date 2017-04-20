#r "../node_modules/fable-core/Fable.Core.dll"

#load "./wrap/util.fs"
#load "./wrap/buffer.fs"
#load "./wrap/q.fs"
#load "./wrap/q-io.fs"
#load "./wrap/serialize.fs"
#load "./wrap/websocket.fs"
#load "./wrap/express.fs"
#load "./wrap/mustache.fs"

let doHello req res nxt =
  QIO.readText "static/index.html"
  |> Q.map
       (fun txt ->
         let helloData =
           Serialize.jsonObject
             [| ("hello", Serialize.jsonString "hello world") |]
         in
         Mustache.render txt helloData
       )
  |> Q.errThen
       (fun e ->
         QIO.readText "static/error.html"
         |> Q.map
              (fun txt ->
                let errorData =
                  Serialize.jsonObject
                    [| ("error", Serialize.jsonString (Util.toString e)) |]
                in
                Mustache.render txt errorData
              )
         |> Q.errThen (fun e -> Util.toString e |> Q.value)
       )
  |> Q.map (fun html -> Express.resEndString html res)
  |> Q.fin
  
let main _ =
  let app = Express.newApp () in
  app
  |> Express.get "/hello" doHello
  |> Express.listen 8008
