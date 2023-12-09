namespace GalaxyQuest.RequestBodies;

/// <summary>
/// Represents the request body for creating a unit.
/// </summary>
public record PostRequestUnitBody(string Id, string Type, string System, string Planet, string DestinationSystem, string DestinationPlanet);