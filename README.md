# StrEnum

StrEnum is a tiny library for creating string-based enums in C#. Use strings instead of integers for richer, more meaningful data while keeping type safety. Read more about when and why to use string enums in [this blog post](https://www.dmitrykhmara.com/blog/string-enums-in-csharp-when-why-and-how).

StrEnum targets .NET Standard 2.0 and has no external dependencies.

## Ecosystem

These companion packages plug StrEnum into the rest of the .NET ecosystem:

- [StrEnum.AspNetCore](https://github.com/StrEnum/StrEnum.AspNetCore/) — bind string enums in ASP.NET Core controllers.
- [StrEnum.EntityFrameworkCore](https://github.com/StrEnum/StrEnum.EntityFrameworkCore/) — use string enums in EF Core queries and migrations.
- [StrEnum.Npgsql](https://github.com/StrEnum/StrEnum.Npgsql/) — map string enums to native Postgres enum types via the Npgsql ADO.NET driver.
- [StrEnum.Npgsql.EntityFrameworkCore](https://github.com/StrEnum/StrEnum.Npgsql.EntityFrameworkCore/) — Postgres-enum mapping for EF Core, on top of StrEnum.Npgsql.
- [StrEnum.Dapper](https://github.com/StrEnum/StrEnum.Dapper/) — use string enums in Dapper commands and queries.
- [StrEnum.System.Text.Json](https://github.com/StrEnum/StrEnum.System.Text.Json/) — JSON serialization and deserialization with System.Text.Json.

## Installation

Install [StrEnum](https://www.nuget.org/packages/StrEnum/) via the .NET CLI:

```
dotnet add package StrEnum
```

## Usage

### Defining a string enum

Inherit from `StringEnum<TEnum>` and use `Define` to declare members:

```csharp
public class Sport : StringEnum<Sport>
{
    public static readonly Sport TrailRunning = Define("TRAIL_RUNNING");
    public static readonly Sport RoadCycling = Define("ROAD_CYCLING");
}
```

The field name becomes the member's name; the argument to `Define` becomes its value.

### Working with members

Members behave like a regular enum — including `ToString`, casting, and equality:

```csharp
var sport = Sport.TrailRunning;

sport.ToString();           // "TrailRunning"
(string)sport;              // "TRAIL_RUNNING"
sport == Sport.RoadCycling; // false
```

### Parsing

`Parse` converts a name or value into the matching member, throwing if no match is found:

```csharp
Sport.Parse("TrailRunning");  // Sport.TrailRunning
Sport.Parse("TRAIL_RUNNING"); // Sport.TrailRunning
Sport.Parse("Quidditch");     // throws ArgumentException: "Requested name or value 'Quidditch' was not found."
```

Pass `ignoreCase: true` for case-insensitive matching:

```csharp
Sport.Parse("trailrunning",  ignoreCase: true); // Sport.TrailRunning
Sport.Parse("trail_running", ignoreCase: true); // Sport.TrailRunning
```

Pass `MatchBy.ValueOnly` to match by value and skip member names:

```csharp
Sport.Parse("TRAIL_RUNNING", matchBy: MatchBy.ValueOnly); // Sport.TrailRunning
Sport.Parse("TrailRunning",  matchBy: MatchBy.ValueOnly); // throws ArgumentException
```

Use `TryParse` for the non-throwing variant:

```csharp
Sport.TryParse("TrailRunning", out var trailRunning); // true,  trailRunning == Sport.TrailRunning
Sport.TryParse("Quidditch",    out var quidditch);    // false, quidditch == null
```

### Listing members

`GetMembers` returns members in declaration order:

```csharp
Sport.GetMembers(); // [Sport.TrailRunning, Sport.RoadCycling]
```

### Switch statements and expressions

Because string enum members aren't constants, use property patterns with `when` guards:

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

The same pattern works in `switch` expressions:

```csharp
var sportName = sport switch
{
    _ when sport == Sport.TrailRunning => "Trail Running",
    _ when sport == Sport.RoadCycling  => "Road Cycling",
    _                                  => "Not yet known"
};
```

### Adding members at runtime

`Define` is `protected`, but you can expose it through a public method to add members after the class is initialized:

```csharp
public class FictionalSport : StringEnum<FictionalSport>
{
    public static readonly FictionalSport Quidditch = Define("QUIDDITCH");

    public static FictionalSport Add(string name, string value) => Define(value, name);
}
```

Both `name` and `value` must be supplied for members added this way:

```csharp
var podracing = FictionalSport.Add("Podracing", "PODRACING");

(string)podracing;                            // "PODRACING"
FictionalSport.Parse("Podracing").ToString(); // "Podracing"
```

## License

Copyright &copy; 2026 [Dmytro Khmara](https://dmytrokhmara.com).

StrEnum is licensed under the [MIT license](LICENSE.txt).
