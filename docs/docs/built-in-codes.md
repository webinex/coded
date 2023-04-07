---
sidebar_position: 2
---

# Built in Codes

## Bult-in codes

| Code   | HTTP Code |
| ------ | --------- |
| INV    | 400       |
| NTFND  | 404       |
| UNAUTH | 401       |
| FRBD   | 403       |
| CNFLT  | 409       |
| LCKD   | 423       |
| UNEXP  | 500       |
| AGGR   | -         |

> Codes support inheritance:  
> `INV.USER_NAME` by default, will be converted to HTTP 400

## Adding custom factory methods

Built in codes has appropriate static methods in `CodedResult`, `CodedFailure` and `CodedException`, like:
`CodedResult.Invalid()`, `CodedFailure.Invalid()`, `CodedException.Invalid()`  

To add custom factory method to `CodedFailure` and `CodedResult` you might create extension methods

```csharp title="AppCodeFailures.cs"
namespace Webinex.Coded;

public static class ThisCodedFailureExtensions
{
    public static CodedFailure<string> UserExists(this ThisCodedFailure _, string email)
    {
        return new CodedFailure<string>(Code.INVALID.Child("USER.EXISTS"), email);
    }
}
```

And use it

```csharp title="UserService.cs"
public class UserService
{
    private readonly IUserRepository _userRepository;

    // ...

    public async Task AddAsync(User user)
    {
        if (await _userRepository.ExistsAsync(user.Email))
            throw CodedFailure.This.UserExists(user.Email).Throw();

        // ...
    }
}
```

## Adding custom base code

```csharp title="Startup.cs"
services
    .AddCodedFailures(x => x
      .AddCode("NT_IMPL", HttpStatusCode.NotImplemented));
```

> `NT_IMPL` would support inheritance. `NT_IMPL.ADD_USER` would be conveted to `HttpStatusCode.NotImplemented`.

## Override status code inheritance

```csharp title="Startup.cs"
services
    .AddCodedFailures(x => x
      .AddCode("INV.CONFLICT", HttpStatusCode.Conflict));
```
> `INV.CONFLICT` would support inheritance. `INV.CONFLICT.MODIFICATION` would be conveted to `HttpStatusCode.Conflict`.