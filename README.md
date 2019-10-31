# Fable + Preact.js + Serverless

Fable SPA deployed to Azure functions.

- The static content is all stored in the function app and served from it
- Gives an example of how the domain can be shared between both front end and back end
- Gives an example of how to post json to the server and wait for it to come back
- Shows the non blocking UI
- Shows how hashing can be incorporated in the build pipeline

There are 3 x Projects:

#### SharedDomain
Contains a simple `User` type that used on front end and the back end. Makes use of `Thoth.Json` & `Thoth.Json.Net`.

#### FableApp
Fable App using `preact.js` for dom rendering & `Thoth.Fetch` for Fetch API. If deploying somewhere other than locally you will need to set the correct end point url on the `Model.PostUrl`.

Makes use of [HtmlWebpackPlugin](https://github.com/jantimon/html-webpack-plugin) & [CleanWebpackPlugin](https://github.com/johnagan/clean-webpack-plugin) for hashing of `bundle.js`

One thing to note is never `open Fable.Core.JS` as it does not play nicely with `System`, `Promise` etc.

#### FunctionApp
2 x Function end points: 1 for static content, the other for json.

Makes use of `.fsproj` [globbing](https://github.com/Microsoft/VSProjectSystem/blob/master/doc/overview/globbing_behavior.md)

### Install the Azure Functions Core Tools package
```
npm install -g azure-functions-core-tools
```

### Build the project with

```
dotnet fsi build.fsx
```
Or build the `FableApp` project first, followed by the `FunctionApp` project.

### Run the serverless functions locally with
```
cd FunctionApp; func start
```

### Build + Run
Build and start the function app running locally in one go
```
dotnet fsi run.fsx
```
Deploy to Azure Functions :satisfied: live app [here](https://fableserverless.azurewebsites.net/api/public/)