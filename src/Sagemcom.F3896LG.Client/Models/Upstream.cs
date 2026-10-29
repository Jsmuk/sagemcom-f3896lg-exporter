namespace Sagemcom.F3896LG.Client.Models;

public sealed record Upstream
{
    public required IReadOnlyList<UpstreamChannel> Channels { get; init; }
}

public sealed record UpstreamChannel
{
    public required int ChannelId { get; init; }
    public long? Frequency { get; init; }
    public long? ChannelWidth { get; init; }
    public required bool LockStatus { get; init; }
    public required double Power { get; init; }
    public int? SymbolRate { get; init; }
    public required string Modulation { get; init; }
    public required string ChannelType { get; init; }
    public string? FftType { get; init; }
    public int? NumberOfActiveSubCarriers { get; init; }
    public int? FirstActiveSubcarrier { get; init; }
    public int? T1Timeout { get; init; }
    public int? T2Timeout { get; init; }
    public int? T3Timeout { get; init; }
    public int? T4Timeout { get; init; }
}