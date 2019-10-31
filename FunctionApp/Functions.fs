namespace Fable.Serverless

open System.IO
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open SharedDomain

module Server =

    // The RequestObjects are new since I last looked at functions.
    // Previously I used something like
    // let r = new HttpResponseMessage()
    // r.StatusCode <- HttpStatusCode.OK
    // r.Content <- new StreamContent(file)
    // r.Content.Headers.ContentType <- Headers.MediaTypeHeaderValue("text/html")

    let [<Literal>] MIMEJSON = "application/json"

    let serveStaticContent (log : ILogger) (context : ExecutionContext) (fileName : string)  =
        let filePath = Path.Combine(context.FunctionAppDirectory, "public", fileName) |> Path.GetFullPath
        try
            let file = new FileStream(filePath, FileMode.Open)
            log.LogInformation <| sprintf "File found: %s" filePath
            OkObjectResult file :> ObjectResult
        with _ ->
            let msg = sprintf "File not found: %s" filePath
            log.LogError msg
            BadRequestObjectResult msg :> ObjectResult

    [<FunctionName("serveStatic")>]
    let serveStatic ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "public/{staticFile?}")>] req : HttpRequest,
                     log : ILogger,
                     context : ExecutionContext) =
        log.LogInformation "Serving static content"
        match req.Path with
        | s when s.HasValue && s.Value = "/api/public/" -> "index.html" |> serveStaticContent log context
        | s -> s.Value.Replace("/api/public/", "") |> serveStaticContent log context

    [<FunctionName("serveJSON")>]
    let serveJSON ([<HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "json")>] req : HttpRequest, log : ILogger) =
        log.LogInformation "Serving JSON"
        match req.ContentType with
        | MIMEJSON ->
            let sr = new StreamReader(req.Body)
            let postedModel = sr.ReadToEnd() |> User.Decode
            let msg = sprintf "Hello %s. I see you can count to %i" postedModel.Name postedModel.Count
            let pause = (System.Math.Abs postedModel.Count) * 1000
            System.Threading.Thread.Sleep pause // Sleep for 5s to show non-blocking UI
            { postedModel with Message = msg }
            |> User.Encode
            |> OkObjectResult
            :> ObjectResult
        | _ ->
            sprintf "Incorrect mime type. I was expecting json but got: %s" req.ContentType
            |> BadRequestObjectResult
            :> ObjectResult