namespace Sagemcom.F3896LG.Client.Models;

public sealed record UpstreamResponse
{
    public required Upstream Upstream { get; init; }
}