namespace Fable.Serverless

open System.IO
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging

module Server =

    let [<Literal>] Indexhtml = "index.html"
    let [<Literal>] Bundlejs = "bundle.js"
    let [<Literal>] Favicon = "favicon.ico"

    // The RequestObjects are new since I last looked at functions.
    // Previously I used something like
    // let r = new HttpResponseMessage()
    // r.StatusCode <- HttpStatusCode.OK
    // r.Content <- new StreamContent(file)
    // r.Content.Headers.ContentType <- Headers.MediaTypeHeaderValue("text/html")

    let serveStaticContent (fileName : string) (log : ILogger) (context : ExecutionContext) =
        let filePath = Path.Combine(context.FunctionAppDirectory, "public", fileName) |> Path.GetFullPath
        try
            let file = new FileStream(filePath, FileMode.Open)
            log.LogInformation <| sprintf "File found: %s" filePath
            OkObjectResult file :> ObjectResult
        with _ ->
            let msg = sprintf "File not found: %s" filePath
            log.LogError msg
            BadRequestObjectResult msg :> ObjectResult

    [<FunctionName("public")>]
    let serveIndexhtml ([<HttpTrigger(AuthorizationLevel.Anonymous, "get")>] req : HttpRequest, log : ILogger, context : ExecutionContext) =
        log.LogInformation "Serving static site."
        serveStaticContent Indexhtml log context

    [<FunctionName("serveFavicon")>]
    let serveFavicon ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Favicon)>] req : HttpRequest, log : ILogger, context : ExecutionContext) =
        log.LogInformation "Serving Favicon"
        serveStaticContent Favicon log context

    [<FunctionName("serveBundlejs")>]
    let serveBundlejs ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Bundlejs)>] req : HttpRequest, log : ILogger, context : ExecutionContext) =
        log.LogInformation "Serving js"
        serveStaticContent Bundlejs log context