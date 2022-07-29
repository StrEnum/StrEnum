# StrEnum

StrEnum is a tiny library that allows to create string-based enums in C#. Give your data more meaning by using strings over numerics while retaining type safety.

StrEnum can be used with the various libraries and frameworks:

- [StrEnum.AspNetCore](https://github.com/StrEnum/StrEnum.AspNetCore/) allows to use string enums in your ASP.NET Core controllers.
- [StrEnum.EntityFrameworkCore](https://github.com/StrEnum/StrEnum.EntityFrameworkCore/) allows to use string enums within EF Core queries and migrations.
- [StrEnum.System.Text.Json](https://github.com/StrEnum/StrEnum.System.Text.Json/) enables JSON serialization and deserialization with System.Text.Json.

StrEnum targets .NET Standard 2.0 and has no external dependencies.

## Installation

You can install [StrEnum](https://www.nuget.org/packages/StrEnum/) using the .NET CLI:

```
dotnet add package StrEnum
```

## Usage

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

To make `Parse` only match the members by value and not by name, pass `MatchBy.ValueOnly` as the third argument:

```csharp
Sport.Parse("TRAIL_RUNNING", matchBy: MatchBy.ValueOnly); // Sport.TrailRunning

Sport.Parse("TrailRunning", matchBy: MatchBy.ValueOnly); // throws an ArgumentException: "Requested value 'TrailRunning' was not found."
```

Use the `TryParse` method when you want to check whether the provided string can be converted to a string enum:

```csharp
Sport.TryParse("TrailRunning", out var trailRunning); // true

(string)trailRunning; // Sport.TrailRunning
    
Sport.TryParse("Quidditch", out var quidditch); // false

quidditch == null; // true
```

### Listing members

Use the `GetMembers` method to list the members of a string enum in the order of definition:

```csharp
Sport.GetMembers(); // [Sport.TrailRunning, Sport.RoadCycling]
```

### Using `switch` statements

You can use StrEnum enums with `switch` statements with the help of C# 8 property patterns and `when` case guards:

```csharp
switch (sport)
{
    case { } when sport == Sport.TrailRunning:
        PutOnTrailShoes();
        break;
    case { } when sport == Sport.RoadCycling:
        GetOnRoadBike();
        break;
}
```

You can also use `switch` expressions like this:

```csharp
var sportName = sport switch
{
    _ when sport == Sport.TrailRunning => "Trail Running",
    _ when sport == Sport.RoadCycling => "Road Cycling",
    _ => "Not yet known"
};
```

### Adding members after initialization

The `Define` method can be used to add members to a string enum after it has been initialized. Since `Define` is protected, you need to expose it to the client code first:

```csharp
public class FictionalSport : StringEnum<FictionalSport>
{
    public static readonly FictionalSport Quidditch = Define("QUIDDITCH");

    public static FictionalSport Add(string name, string value)
    {
        return Define(value, name);
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