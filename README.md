# StrEnum

StrEnum is a tiny library that allows to create string-based enums in C#. Give your data more meaning by using strings over numerics while retaining type safety.

StrEnum targets .NET Standard 2.0 and has no external dependencies.

StrEnum-based enums can be used with EF Core with the help of the [StrEnum.EntityFrameworkCore](https://github.com/StrEnum/StrEnum.EntityFrameworkCore/) package.

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
public class Sport : StringEnum<Sport>
{
    public static readonly Sport TrailRunning = Define("TRAIL_RUNNING");
    public static readonly Sport RoadCycling = Define("ROAD_CYCLING");
}
```

You can now use your string enum as you would a regular enum:

```csharp
var sport = Sport.TrailRunning;

sport.ToString(); // TrailRunning

(string)sport; // TRAIL_RUNNING

sport == Sport.RoadCycling; // false
```

### Parsing

Use the `Parse` method to convert the name or a string value to the corresponding string enum member:

```csharp
Sport.Parse("TrailRunning"); // Sport.TrailRunning

Sport.Parse("TRAIL_RUNNING"); // Sport.TrailRunning

Sport.Parse("Quidditch"); // throws an ArgumentException: "Requested name or value 'Quidditch' was not found."
```

To make `Parse` ignore case, pass `true` as the second argument:

```csharp
Sport.Parse("trailrunning", ignoreCase: true); // Sport.TrailRunning

Sport.Parse("trail_running", ignoreCase: true); // Sport.TrailRunning
```

Use the `TryParse` method when you want to check whether the provided string can be converted to a string enum:

```csharp
Sport.TryParse("TrailRunning", out var trailRunning); // true

(string)trailRunning; // Sport.TrailRunning
    
Sport.TryParse("Quidditch", out var quidditch); // false

quidditch == null; // true
```

### Adding members after initialization

The `Define` method can be used to add members to a string enum after it has been initialized. Since `Define` is protected, you need to expose it to the client code first:

```csharp
public class FictionalSport : StringEnum<FictionalSport>
{
    public static readonly FictionalSport Quidditch = Define("QUIDDITCH");

    public static FictionalSport Add(string name, string code)
    {
        return Define(code, name);
    }
}
```

You need to provide both name and value for the members defined in such way:

```csharp
var podracing = FictionalSport.Add("Podracing", "PODRACING");

(string)podracing; // PODRACING

FictionalSport.Parse("Podracing").ToString(); // Podracing
```

## License

Copyright &copy; 2022 [Dmitry Khmara](https://dmitrykhmara.com).

StrEnum is licensed under the [MIT license](LICENSE.txt).