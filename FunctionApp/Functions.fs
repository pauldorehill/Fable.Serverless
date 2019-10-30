namespace Fable.Serverless

open System.IO
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging

module Server =

    // The RequestObjects are new since I last looked at functions.
    // Previously I used something like
    // let r = new HttpResponseMessage()
    // r.StatusCode <- HttpStatusCode.OK
    // r.Content <- new StreamContent(file)
    // r.Content.Headers.ContentType <- Headers.MediaTypeHeaderValue("text/html")

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
    let serveStatic ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{staticFile?}")>] req : HttpRequest, log : ILogger, context : ExecutionContext) =
        log.LogInformation "Serving static content"
        match req.Path with
        | s when s.HasValue && s.Value = "/api/" -> "index.html" |> serveStaticContent log context
        | s -> s.Value.Replace("/api/", "") |> serveStaticContent log context