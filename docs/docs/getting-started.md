---
sidebar_position: 1
---

# Getting started

## Installation

```shell
dotnet add package Webinex.Coded
dotnet add package Webinex.Coded.AspNetCore
```

## Project setup

```csharp title="Startup.cs"
public void ConfigureServices(IServiceCollection services)
{
    // ...
    services
        .AddCodedFailures()
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ...
    app.UseCodedExceptions()
}
```

### Start using it

```csharp title="UserService.cs"
public class UserService
{
    public async Task AddAsync(User user)
    {
        if (user.Email == null)
            throw CodedException.Invalid(new { user.Email });

        // ...
    }
}
```
