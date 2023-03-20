# Coded exeptions
____
This library is a pattern of exceptions usages as coded failures.
You don't create custom types for exceptions, instead of it - you write new codes.  

Exception can contain Code / Default Message / Payload. Exceptions catches in middleware
and this data would be passed to back to client in http header.  

Take a look on CodedExceptionCodes.cs, it contain default exception codes.  

> IMPORTANT:
> They are inheritable.  
> For example,  
> we have "INV" = HTTP 400 (by default configuration),  
> "INV.EMAIL" would return HTTP 400 if "INV.EMAIL" code not provided.  
> If "INV.EMAIL" = HTTP 403  
> than "INV.EMAIL.WRNG" = HTTP 403

You can add you custom mappings by calling .Add on configuration
```c#
services
      .AddCodedExceptions(x => x
          .AddCode("INV.EMAIL", HttpStatusCode.Forbidden))
```

By default, all non-coded exceptions would be converted to UNEXPECTED coded exceptions.  
If you don't like it, you can clear `configuration.Converters` collection and add your own converter as last.  

If all exception converters returns non-succeed result - exception rethrown.  

You can add exceptions to coded exceptions mappings (useful for 3rd party exceptions or your custom, like ValidationException).  
First, you need to create converter
```c#
class ValidationExceptionConverter : CodedExceptionConverter<ValidationException>
{
    protected override ConvertResult ConvertException(ValidationException ex)
    {
        var codedException = new CodedException(CodedExceptionCodes.INVALID);
        return ConvertResult.Success(codedException);
    }
}
```
And add it in configuration
```c#
services
    // order of execution in accordance with registration is guaranted
    .AddFirst<ValidationExceptionConverter>(ServiceLifetime.Scoped);
```