namespace Sagemcom.F3896LG.Client.Models;

public sealed record Downstream
{
    public required IReadOnlyList<DownstreamChannel> Channels { get; init; }
}

public sealed record DownstreamChannel
{
    public required string ChannelType { get; init; }
    public required int ChannelId { get; init; }
    public long? Frequency { get; init; }
    public long? ChannelWidth { get; init; }
    public required double Power { get; init; }
    public required string Modulation { get; init; }
    public double? Snr { get; init; }
    public required double RxMer { get; init; }
    public required long CorrectedErrors { get; init; }
    public required long UncorrectedErrors { get; init; }
    public required bool LockStatus { get; init; }
    public string? FftType { get; init; }
    public int? NumberOfActiveSubCarriers { get; init; }
    public int? FirstActiveSubcarrier { get; init; }
}