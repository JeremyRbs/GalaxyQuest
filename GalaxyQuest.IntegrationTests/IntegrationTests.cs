using Microsoft.AspNetCore.Mvc.Testing;
using Shard.GalaxyQuest;
using Shard.Shared.Web.IntegrationTests;
using Xunit.Abstractions;

namespace GalaxyQuest.IntegrationTests
{
    public class IntegrationTests : BaseIntegrationTests<Program>
    {
        public IntegrationTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) : 
            base(factory, testOutputHelper)
        {
        }
    }
}