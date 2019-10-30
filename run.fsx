// Build + Start function app

#load "build.fsx"
Build.shellExecute "cd FunctionApp; func start"
