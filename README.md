# StrEnum

StrEnum is a tiny library that allows to create string-based enums in C#. Using strings instead of numerics gives your state more meaning when it crosses the boundaries of your .NET application.

StrEnum targets .NET Standard 2.0 and has no external dependencies.

## How do I install it?

You can install [StrEnum](https://www.nuget.org/packages/StrEnum/) using the .NET CLI:

```
dotnet add package StrEnum
```

Or using the Package Manager console:

```
PM> Install-Package StrEnum
```

## How do I use it?

Create a class that inherits from the `StringEnum<TEnum>` class and use the `Define` method to define the members:

```csharp
public class Country : StringEnum<Country>
{
    public static readonly Country Ukraine = Define("UKR");
    public static readonly Country SouthAfrica = Define("ZAF");
}
```

You can now use your string enum as you would a regular enum:

```csharp
var country = Country.Ukraine;

country.ToString(); // Ukraine

(string)country; // UKR

country == Country.SouthAfrica; // false
```

### Parsing

Use the `Parse` method to convert the name or a string value to a corresponding string enum member:

```csharp
Country.Parse("Ukraine"); // Country.Ukraine

Country.Parse("UKR"); // Country.Ukraine

Country.Parse("Gondor"); // throws an ArgumentException: "Requested name or value 'Gondor' was not found."
```

### Adding members after initialization

The `Define` method can be used to add members to a string enum after it has been initialized. Since `Define` is protected, you need to expose it to the client code first:

```csharp
public class FictionalCountry : StringEnum<FictionalCountry>
{
    public static readonly FictionalCountry Gondor = Define("GDR");

    public static FictionalCountry Add(string name, string code)
    {
        return Define(code, name);
    }
}
```

You need to provide both name and value for the members defined in such way:

```csharp
var rohan = FictionalCountry.Add("Rohan", "RHN");

(string)rohan; // RHN

FictionalCountry.Parse("Rohan").ToString(); // Rohan
```

## License

Copyright &copy; 2022 [Dmitry Khmara](https://dmitrykhmara.com).

StrEnum is licensed under the [MIT license](LICENSE.txt).