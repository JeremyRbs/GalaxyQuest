<<<<<<< HEAD
﻿using System.Net;

namespace Shard.Shared.Web.IntegrationTests;
=======
﻿namespace Shard.Shared.Web.IntegrationTests;
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a

public partial class BaseIntegrationTests<TEntryPoint, TWebApplicationFactory>
{
    private async Task<string> CreateNewUserPath()
    {
        var userId = Guid.NewGuid().ToString();
<<<<<<< HEAD
        using var client = factory.CreateClient();
=======
        using var client = CreateClient();
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
        using var userCreationResponse = await client.PutAsJsonAsync("users/" + userId, new
        {
            id = userId,
            pseudo = "johny"
        });
        await userCreationResponse.AssertSuccessStatusCode();

        return "users/" + userId;
    }

<<<<<<< HEAD
    private async Task<Unit> GetScout(string userPath)
    {
        using var client = factory.CreateClient();
        using var unitsResponse = await client.GetAsync($"{userPath}/units");
        await unitsResponse.AssertSuccessStatusCode();

        var units = await unitsResponse.AssertSuccessJsonAsync();
        return new (units[0]);
    }
=======
    private Task<Unit> GetScout(string userPath)
        => GetSingleUnitOfType(userPath, "scout");
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "2")]
<<<<<<< HEAD
    public async Task CreatingUserCreatesScout()
    {
        var unit = await GetScout(await CreateNewUserPath());
        Assert.Equal("scout", unit.Type);
    }
=======
    public Task CreatingUserCreatesScout()
        => CreatingUserCreatesOneUnitOfType("scout");
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "2")]
    public async Task CreatingUserCreatesScoutInSomeSystem()
    {
        var unit = await GetScout(await CreateNewUserPath());
        Assert.NotNull(unit.System);
    }

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "2")]
    public async Task CreatingUserCreatesScoutInSomeExistingSystem()
    {
        var unit = await GetScout(await CreateNewUserPath());

<<<<<<< HEAD
        using var client = factory.CreateClient();
=======
        using var client = CreateClient();
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
        using var response = await client.GetAsync("systems");

        var systems = new StarSystems(await response.AssertSuccessJsonAsync());
        var system = systems.SingleOrDefault(system => system.Name == unit.System);
        Assert.NotNull(system);
    }

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "2")]
<<<<<<< HEAD
    public async Task GettingScoutStatusById()
    {
        var userPath = await CreateNewUserPath();
        var unit = await GetScout(userPath);

        using var client = factory.CreateClient();
        using var response = await client.GetAsync($"{userPath}/units/{unit.Id}");
        await response.AssertSuccessStatusCode();

        var unit2 = await response.Content.ReadAsAsync<JObject>();
        Assert.Equal(unit.ToString(), unit2?.ToString());
    }
=======
    public Task GettingScoutStatusById()
        => GettingUnitStatusById("scout");
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "2")]
<<<<<<< HEAD
    public async Task GettingScoutStatusWithWrongIdReturns404()
    {
        var userPath = await CreateNewUserPath();
        var unit = await GetScout(userPath);

        using var client = factory.CreateClient();
        using var response = await client.GetAsync($"{userPath}/units/{unit.Id}z");
        await response.AssertStatusEquals(HttpStatusCode.NotFound);
    }
=======
    public Task GettingScoutStatusWithWrongIdReturns404()
        => GettingUnitStatusWithWrongIdReturns404("scout");
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "2")]
<<<<<<< HEAD
    public async Task MoveScoutToOtherSystem()
    {
        var userPath = await CreateNewUserPath();
        var unit = await GetScout(userPath);

        var destinationSystem = await GetRandomSystemOtherThan(unit.System);

        using var client = factory.CreateClient();
        using var response = await client.PutAsJsonAsync($"{userPath}/units/{unit.Id}", new
        {
            id = unit.Id,
            type = "scout",
            system = destinationSystem
        });

        var unitAfterMove = new Unit(await response.AssertSuccessJsonAsync());
        Assert.NotNull(unitAfterMove);
        Assert.Equal(unit.Id, unitAfterMove.Id);
        Assert.Equal(destinationSystem, unitAfterMove.System);
    }

    private async Task<string> GetRandomSystemOtherThan(string systemName)
    {
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("systems");

        var systems = new StarSystems(await response.AssertSuccessJsonAsync());
        var system = systems.First(system => system.Name != systemName);

        return system.Name;
    }
=======
    public Task MoveScoutToOtherSystem()
        => MoveUnitToOtherSystem("scout");
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "2")]
<<<<<<< HEAD
    public async Task MoveScoutToPlanet()
    {
        var userPath = await CreateNewUserPath();
        var unit = await GetScout(userPath);

        var destinationPlanet = await GetSomePlanetInSystem(unit.System);

        using var client = factory.CreateClient();
        using var response = await client.PutAsJsonAsync($"{userPath}/units/{unit.Id}", new
        {
            id = unit.Id,
            type = "scout",
            system = unit.System,
            planet = destinationPlanet
        });

        var unitAfterMove = new Unit(await response.AssertSuccessJsonAsync());
        Assert.Equal(unit.Id, unitAfterMove.Id);
        Assert.Equal(unit.System, unitAfterMove.System);
        Assert.Equal(destinationPlanet, unitAfterMove.Planet);
    }

    private async Task<string> GetSomePlanetInSystem(string systemName)
    {
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("systems");

        var systems = new StarSystems(await response.AssertSuccessJsonAsync());
        var system = systems.SingleOrDefault(system => system.Name == systemName);
        Assert.NotNull(system);

        var planet = system.Planets.FirstOrDefault();
        Assert.NotNull(planet);
        return planet.Name;
    }
=======
    public Task MoveScoutToPlanet()
        => MoveUnitToPlanet("scout");
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "2")]
    public async Task AskingCurrentLocationsReturnsDetails()
    {
        var userPath = await CreateNewUserPath();
        var unit = await GetScout(userPath);
        
        var destinationPlanet = await GetSomePlanetInSystem(unit.System);

<<<<<<< HEAD
        using var client = factory.CreateClient();
        using var moveResponse = await client.PutAsJsonAsync($"{userPath}/units/{unit.Id}", new
        {
            id = unit.Id,
            type = "scout",
            system = unit.System,
            planet = destinationPlanet
        });
        await moveResponse.AssertSuccessStatusCode();

=======
        unit.DestinationPlanet = destinationPlanet;

        using var client = CreateClient();
        using var moveResponse = await client.PutTestEntityAsync($"{userPath}/units/{unit.Id}", unit);
        await moveResponse.AssertSuccessStatusCode();

        await fakeClock.Advance(new TimeSpan(0, 0, 15));

>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
        using var scoutingResponse = await client.GetAsync($"{userPath}/units/{unit.Id}/location");

        var location = (await scoutingResponse.AssertSuccessJsonAsync()).AssertObject();
        Assert.Equal(unit.System, location["system"].AssertString());
        Assert.Equal(destinationPlanet, location["planet"].AssertString());

<<<<<<< HEAD
        foreach (var key in location["resourcesQuantity"].AssertObject().Keys)
=======
        AssertResourcesQuantity(location);
    }

    private static void AssertResourcesQuantity(JObjectAsserter data)
    {
        foreach (var key in data["resourcesQuantity"].AssertObject().Keys)
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
        {
            Assert.Contains(key, new[]
            {
                "carbon",
                "iron",
                "gold",
                "aluminium",
                "titanium",
                "water",
                "oxygen",
            });
        }
    }
<<<<<<< HEAD
=======

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "3")]
    public Task GetScout_IfMoreThan2secAway_DoesNotWait()
        => GetUnit_IfMoreThan2secAway_DoesNotWait("scout");

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "3")]
    public Task GetScout_IfLessOrEqualThan2secAway_Waits()
        => GetUnit_IfLessOrEqualThan2secAway_Waits("scout");

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "3")]
    public Task GetScout_IfLessOrEqualThan2secAway_WaitsUntilArrived()
        => GetUnit_IfLessOrEqualThan2secAway_WaitsUntilArrived("scout");
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
}
