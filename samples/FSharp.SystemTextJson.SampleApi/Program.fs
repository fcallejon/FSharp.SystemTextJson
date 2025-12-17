module Program

type Marker = class end

open System
open System.Text.Json
open System.Text.Json.Serialization
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

type GreetingRequest = { Name: string }

type GreetingResponse = { Message: string }

let configureJsonOptions (options: JsonSerializerOptions) =
    options.UnmappedMemberHandling <- JsonUnmappedMemberHandling.Skip
    JsonFSharpOptions.Default()
        .AddToJsonSerializerOptions(options)

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)

    builder.Services
        .ConfigureHttpJsonOptions(fun options -> configureJsonOptions options.SerializerOptions)
    |> ignore

    let app = builder.Build()

    app.MapPost(
        "/greeting",
        Func<GreetingRequest, IResult>(fun request ->
            let response = { Message = $"Hello, {request.Name}!" }
            Results.Ok(response))
    )
    |> ignore

    app.Run()
    0
