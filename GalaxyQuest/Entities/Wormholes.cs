namespace GalaxyQuest.Entities;

/// <summary>
/// Represents information associated with a wormhole.
/// </summary>
public record class Wormholes
{
    /// <summary>
    /// Gets or sets the base URI of the wormhole.
    /// </summary>
    public Uri BaseUri { get; init; }

    /// <summary>
    /// Gets or sets the system associated with the wormhole.
    /// </summary>
    public string System { get; init; }

    /// <summary>
    /// Gets or sets the user associated with the wormhole.
    /// </summary>
    public string User { get; init; }

    /// <summary>
    /// Gets or sets the shared password associated with the wormhole.
    /// </summary>
    public string SharedPassword { get; init; }
}