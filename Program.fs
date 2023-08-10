open System.Threading.Tasks
open NetVips
open System.IO

let convertToAvif () =
    try
        use image =
            Image.NewFromFile("img1.jpg", access = Enums.Access.SequentialUnbuffered)

        image.Heifsave("./output/img1-avif.avif", effort = 3, q = 80, compression = Enums.ForeignHeifCompression.Av1)
    with err ->
        printfn "Error in convertToAvif: %A" err

let convertToThumbnail () =
    try
        use image =
            // Doing it this way seems to use less memory
            Image.ThumbnailBuffer(
                File.ReadAllBytes("img1.jpg"),
                width = 100,
                height = 100,
                crop = Enums.Interesting.Attention
            )

        image.Webpsave("./output/img1-thumbnail.webp", effort = 2, q = 70)
    with err ->
        printfn "Error in convertToThumbnail: %A" err

let convertImageToWebp () =
    try
        use image =
            Image.NewFromFile("img1.jpg", access = Enums.Access.SequentialUnbuffered)

        image.Webpsave("./output/img1-webp.webp", effort = 3, q = 80)
    with err ->
        printfn "Error in convertImageToWebp: %A" err

[<EntryPoint>]
let main _ =
    let arr = Array.init 3000 (fun v -> v)

    Parallel.ForEach(
        arr,
        new ParallelOptions(MaxDegreeOfParallelism = 4),
        (fun _ _ index ->
            convertImageToWebp ()
            convertToThumbnail ()
            convertToAvif ()
            printfn $"Done {index + 1L} of {arr.Length}")
    )
    |> ignore

    0
