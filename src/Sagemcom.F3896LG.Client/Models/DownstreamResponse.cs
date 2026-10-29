namespace Sagemcom.F3896LG.Client.Models;

public sealed record DownstreamResponse
{
    public required Downstream Downstream { get; init; }
}