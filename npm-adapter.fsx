open System
open System.IO
open System.Text.RegularExpressions

let args = Environment.GetCommandLineArgs() |> Array.skip 2 |> Array.toList

let sourceDir = args.[0]

type Mapper =
    {
        Original: string
        Mapped: string
    }

let mappers =
    args
    |> List.skip 1
    |> List.map (fun arg ->
        let parts = arg.Split('=')

        {
            Original = parts.[0]
            Mapped = parts.[1]
        }
    )

printfn "Source dir: %s" sourceDir
printfn "Mappers: %A" mappers

let regex = Regex("""(?<start>import.*from ")(?<fromPath>.*)(?<end>";)""")

let files = Directory.GetFiles(sourceDir, "*.js", SearchOption.AllDirectories)

printfn "Processing files:"

for file in files do
    let fileContent = File.ReadAllText(file)
    printfn "   %s" file

    let newContent =
        regex.Replace(
            fileContent,
            fun m ->
                let start = m.Groups.["start"].Value
                let fromPath = m.Groups.["fromPath"].Value
                let endText = m.Groups.["end"].Value

                let mapper =
                    mappers
                    |> List.tryFind (fun mapper -> fromPath.Contains(mapper.Original))

                let newFromPath =
                    match mapper with
                    | None -> fromPath
                    | Some mapper ->
                        let endIndex =
                            fromPath.IndexOf(mapper.Original) + mapper.Original.Length

                        mapper.Mapped + fromPath.Substring(endIndex)

                $"{start}{newFromPath}{endText}"
        )

    File.WriteAllText(file, newContent)

printfn "Remove original directories:"

for mapper in mappers do
    let searchDir = Path.Combine(sourceDir, mapper.Original)
    if Directory.Exists(searchDir) then
        printfn "   %s" searchDir
        Directory.Delete(searchDir, true)
