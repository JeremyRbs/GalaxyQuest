namespace GalaxyQuest.RequestBodies
{
    /// <summary>
    /// Represents the request body for creating a building.
    /// </summary>
    public record PostRequestBuildingBody(string Type, string BuilderId, string ResourceCategory, string System, string Planet);
}
