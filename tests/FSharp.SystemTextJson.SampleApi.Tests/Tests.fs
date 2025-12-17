module FSharp.SystemTextJson.SampleApi.Tests

open System.Net
open System.Net.Http
open System.Text
open Microsoft.AspNetCore.Mvc.Testing
open Xunit

type ApiFixture() =
    inherit WebApplicationFactory<Program.Marker>()

[<Fact>]
let ``POST /greeting with valid payload returns 200`` () =
    task {
        use factory = new ApiFixture()
        use client = factory.CreateClient()

        let json = """{"Name": "World"}"""
        use content = new StringContent(json, Encoding.UTF8, "application/json")

        let! response = client.PostAsync("/greeting", content)

        Assert.Equal(HttpStatusCode.OK, response.StatusCode)

        let! responseBody = response.Content.ReadAsStringAsync()
        Assert.Contains("Hello, World!", responseBody)
    }

[<Fact>]
let ``POST /greeting with extra property returns 200 when Skip`` () =
    task {
        use factory = new ApiFixture()
        use client = factory.CreateClient()

        let json = """{"Name": "World", "ExtraProperty": "should be ignored"}"""
        use content = new StringContent(json, Encoding.UTF8, "application/json")

        let! response = client.PostAsync("/greeting", content)

        Assert.Equal(HttpStatusCode.OK, response.StatusCode)

        let! responseBody = response.Content.ReadAsStringAsync()
        Assert.Contains("Hello, World!", responseBody)
    }
