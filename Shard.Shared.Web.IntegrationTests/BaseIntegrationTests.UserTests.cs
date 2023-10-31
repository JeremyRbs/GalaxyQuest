using System.Net;

namespace Shard.Shared.Web.IntegrationTests;

public partial class BaseIntegrationTests<TEntryPoint, TWebApplicationFactory>
{

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "2")]
    public async Task CanGet404WhenQueryingUser()
    {
<<<<<<< HEAD
        using var client = factory.CreateClient();
=======
        using var client = CreateClient();
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
        using var response = await client.GetAsync("users/42");
        await response.AssertStatusEquals(HttpStatusCode.NotFound);
    }

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "2")]
    public async Task CanCreateUser()
    {
<<<<<<< HEAD
        using var client = factory.CreateClient();
=======
        using var client = CreateClient();
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
        using var response = await client.PutAsJsonAsync("users/43", new
        {
            id = "43",
            pseudo = "johny"
        });

        var user = (await response.AssertSuccessJsonAsync()).AssertObject();
        Assert.Equal("43", user["id"].AssertString());
        Assert.Equal("johny", user["pseudo"].AssertString());
    }

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "2")]
    public async Task CreatingUserWithInconsistentIdFails()
    {
<<<<<<< HEAD
        using var client = factory.CreateClient();
=======
        using var client = CreateClient();
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
        using var response = await client.PutAsJsonAsync("users/44", new
        {
            id = "45",
            pseudo = "johny"
        });
        await response.AssertStatusEquals(HttpStatusCode.BadRequest);
    }

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "2")]
    public async Task CreatingUserWithLackOfBodyFails()
    {
<<<<<<< HEAD
        using var client = factory.CreateClient();
=======
        using var client = CreateClient();
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
        using var response = await client.PutAsJsonAsync<object?>("users/46", null);
        await response.AssertStatusEquals(HttpStatusCode.BadRequest);
    }

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "2")]
    public async Task CreatingUserWithInvalidIdFails()
    {
<<<<<<< HEAD
        using var client = factory.CreateClient();
=======
        using var client = CreateClient();
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
        using var response = await client.PutAsJsonAsync("users/'", new
        {
            id = "'",
            pseudo = "johny"
        });
        await response.AssertStatusCodeAmong(HttpStatusCode.BadRequest, HttpStatusCode.NotFound);
    }

    [Fact]
    [Trait("grading", "true")]
    [Trait("version", "2")]
    public async Task CanFetchCreatedUser()
    {
<<<<<<< HEAD
        using var client = factory.CreateClient();
=======
        using var client = CreateClient();
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
        using var userCreationResponse = await client.PutAsJsonAsync("users/47", new
        {
            id = "47",
            pseudo = "johny"
        });
        await userCreationResponse.AssertSuccessStatusCode();

        using var getUserResponse = await client.GetAsync("users/47");

<<<<<<< HEAD
        var units = await getUserResponse.AssertSuccessJsonAsync();
        Assert.Equal("47", units["id"].AssertString());
        Assert.Equal("johny", units["pseudo"].AssertString());
    }
=======
        var user = await getUserResponse.AssertSuccessJsonAsync();
        Assert.Equal("47", user["id"].AssertString());
        Assert.Equal("johny", user["pseudo"].AssertString());
	}
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
}
