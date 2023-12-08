namespace GalaxyQuest.RequestBodies;

public record PostRequestUserBody(string Id, string Pseudo, DateTimeOffset DateOfCreation, Dictionary<string, int>? ResourcesQuantity);