# Fable + Preact.js + Serverless

Fable SPA deployed to Azure functions. The static content is all stored in the function app and served from it.

Install the Azure Functions Core Tools package
```
npm install -g azure-functions-core-tools
```

Build the project with

```
dotnet fsi build.fsx
```
Or build the `FableApp` project first, followed by the `FunctionApp` project.

Run the serverless functions locally with

```
cd FunctionApp; func start
```

Note if running from VS the function app runs from the debug/release folders: for some reason the `public` folder contents does not get copied across so the functions will not be able to find any of the files.

Deploy to Azure Functions :satisfied: live app [here](https://fableserverless.azurewebsites.net/api/public)

## Notes

This is just the simple counter App - At some point I'll add some JSON send/receive to show how code sharing can shine.