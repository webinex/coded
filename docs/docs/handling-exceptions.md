---
sidebar_position: 3
---

# Handling Exceptions

Coded exception middleware would handle all exceptions which appear in request pipeline, log it and send coded exception in HTTP response.

Out of the box, coded exception middleware would convert all exceptions except `CodedException` to `HttpStatusCode.Unexpected`. But you can add exception convertion:

```csharp title="ExceptionConverter.cs"
public class ExceptionConverter : ICodedFailureConverter
{
    public ConvertResult Convert(Exception ex)
    {
        return ex switch
        {
            UnauthorizedAccessException _ => ConvertResult.Success(CodedFailure.Unauthorized()),
            NotUniqueEmailException exc => ConvertResult.Success(new CodedFailure("INV.NOT_UNIQUE_EMAIL", new { exc.Email })),
            _ => ConvertResult.Nope(),
        };
    }
}
```

```csharp title="Startup.cs"
services
  .AddCodedFailures(x => x
      .AddFirst<ExceptionConverter>())
```
