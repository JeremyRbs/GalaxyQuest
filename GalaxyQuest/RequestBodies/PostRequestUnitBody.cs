namespace GalaxyQuest.RequestBodies;

public record PostRequestUnitBody(string Id, string Type, string System, string Planet, string DestinationSystem, string DestinationPlanet);