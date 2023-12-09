namespace GalaxyQuest.RequestBodies;

/// <summary>
/// Represents the request body for creating a user.
/// </summary>
public record PostRequestUserBody(string Id, string Pseudo, DateTimeOffset DateOfCreation, Dictionary<string, int>? ResourcesQuantity);