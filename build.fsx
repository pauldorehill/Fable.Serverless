open System.IO
open System.Diagnostics

let goToCurrentDir = "cd " + __SOURCE_DIRECTORY__ + ";"

let shellExecute args =
    let startInfo = ProcessStartInfo()
    startInfo.CreateNoWindow <- true
    startInfo.UseShellExecute <- false
    startInfo.FileName <- "powershell.exe"
    startInfo.Arguments <- goToCurrentDir + args
    startInfo.RedirectStandardOutput <- true
    startInfo.RedirectStandardInput <- true
    let proc = Process.Start startInfo
    while not proc.StandardOutput.EndOfStream do
        printfn "%s" <| proc.StandardOutput.ReadLine()

    printfn "Complete"
    proc.WaitForExit()

// Build Fable project
printfn "Building Fable project"
shellExecute "cd FableApp/src; dotnet build "

printfn "Installing NPM packages"
shellExecute "cd FableApp; npm install"

printfn "Running webpack"
shellExecute "cd FableApp; npm run-script build"

// Build Function App project
printfn "Building FunctionApp project"
shellExecute "dotnet build FunctionApp"