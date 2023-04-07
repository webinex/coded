---
sidebar_position: 4
---

# Usage with HttpClient

`Webinex.Coded` out of the box supports `HttpClient`. When remote service returns failed response with `x-coded-failure` HTTP header, than `CodedExceptionHttpMessageHandler` will deserialize coded exception payload and throw `CodedException`.

```csharp title="Startup.cs"
services
    .AddHttpClient("REMOTE_SERVICE")
    .AddCodedExceptionHttpMessageHandler()
```
