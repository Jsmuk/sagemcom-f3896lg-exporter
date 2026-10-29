
using System.Globalization;
using System.Text;
using Sagemcom.F3896LG.Client.Models;

namespace Sagemcom.F3896LG.Exporter;

public static class PrometheusMetricsFormatter
{
    public static string Render(DownstreamResponse downstream, UpstreamResponse upstream)
    {
        var sb = new StringBuilder();
        AppendChannelLocked(sb, downstream.Downstream, upstream.Upstream);
        AppendPrometheusLine(sb, "");

        AppendChannelPower(sb, downstream.Downstream, upstream.Upstream);
        AppendPrometheusLine(sb, "");

        AppendChannelFrequency(sb, downstream.Downstream, upstream.Upstream);
        AppendPrometheusLine(sb, "");

        AppendChannelSnr(sb, downstream.Downstream);
        AppendPrometheusLine(sb, "");

        AppendChannelRxMerDb(sb, downstream.Downstream);
        AppendPrometheusLine(sb, "");

        AppendTotalCorrectedCodewordErrors(sb, downstream.Downstream);
        AppendPrometheusLine(sb, "");

        AppendTotalUncorrectedCodewordErrors(sb, downstream.Downstream);
        AppendPrometheusLine(sb, "");

        AppendT3TimeoutsTotal(sb, upstream.Upstream);
        AppendPrometheusLine(sb, "");

        AppendT4TimeoutsTotal(sb, upstream.Upstream);
        AppendPrometheusLine(sb, "");

        AppendChannelMetadata(sb, upstream.Upstream, downstream.Downstream);
        AppendPrometheusLine(sb, "");

        return sb.ToString();
    }

    private static void AppendChannelLocked(StringBuilder sb, Downstream downstream, Upstream upstream)
    {
        AppendPrometheusLine(sb, "# HELP docsis_channel_locked Whether the channel is locked (1 = yes, 0 = no).");
        AppendPrometheusLine(sb, "# TYPE docsis_channel_locked gauge");

        foreach (var channel in downstream.Channels)
        {
            AppendPrometheusLine(sb,
                $"docsis_channel_locked{{direction=\"{EscapeLabelValue("downstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\"}} {(channel.LockStatus ? 1 : 0)}");
        }

        foreach (var channel in upstream.Channels)
        {
            AppendPrometheusLine(sb,
                $"docsis_channel_locked{{direction=\"{EscapeLabelValue("upstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\"}} {(channel.LockStatus ? 1 : 0)}");
        }
    }

    private static void AppendChannelPower(StringBuilder sb, Downstream downstream, Upstream upstream)
    {
        AppendPrometheusLine(sb, "# HELP docsis_channel_power_dbmv Channel power in dBmV.");
        AppendPrometheusLine(sb, "# TYPE docsis_channel_power_dbmv gauge");

        foreach (var channel in downstream.Channels)
        {
            var power = channel.ChannelType == "ofdm"
                ? channel.Power / 10.0
                : channel.Power;

            AppendPrometheusLine(sb,
                $"docsis_channel_power_dbmv{{direction=\"{EscapeLabelValue("downstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\"}} {power.ToString(CultureInfo.InvariantCulture)}");
        }

        foreach (var channel in upstream.Channels)
        {
            var power = channel.ChannelType == "ofdma"
                ? channel.Power / 10.0
                : channel.Power;

            AppendPrometheusLine(sb,
                $"docsis_channel_power_dbmv{{direction=\"{EscapeLabelValue("upstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\"}} {power.ToString(CultureInfo.InvariantCulture)}");
        }
    }

    private static void AppendChannelFrequency(StringBuilder sb, Downstream downstream, Upstream upstream)
    {
        AppendPrometheusLine(sb, "# HELP docsis_channel_frequency_hz Channel frequency in Hz.");
        AppendPrometheusLine(sb, "# TYPE docsis_channel_frequency_hz gauge");

        foreach (var channel in downstream.Channels)
        {
            if (channel.Frequency is null)
                continue;

            AppendPrometheusLine(sb,
                $"docsis_channel_frequency_hz{{direction=\"{EscapeLabelValue("downstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\"}} {channel.Frequency}");
        }

        foreach (var channel in upstream.Channels)
        {
            if (channel.Frequency is null)
                continue;

            AppendPrometheusLine(sb,
                $"docsis_channel_frequency_hz{{direction=\"{EscapeLabelValue("upstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\"}} {channel.Frequency}");
        }
    }

    private static void AppendChannelSnr(StringBuilder sb, Downstream downstream)
    {
        AppendPrometheusLine(sb, "# HELP docsis_channel_snr_db Channel signal-to-noise ratio in dB.");
        AppendPrometheusLine(sb, "# TYPE docsis_channel_snr_db gauge");

        foreach (var channel in downstream.Channels)
        {
            if (channel.Snr is null)
                continue;

            AppendPrometheusLine(sb,
                $"docsis_channel_snr_db{{direction=\"{EscapeLabelValue("downstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\"}} {channel.Snr.Value.ToString(CultureInfo.InvariantCulture)}");
        }
    }

    private static void AppendChannelRxMerDb(StringBuilder sb, Downstream downstream)
    {
        AppendPrometheusLine(sb, "# HELP docsis_channel_rx_mer_db Channel receive modulation error ratio in dB.");
        AppendPrometheusLine(sb, "# TYPE docsis_channel_rx_mer_db gauge");

        foreach (var channel in downstream.Channels)
        {
 var rxMer = channel.ChannelType == "ofdm"
                ? channel.RxMer / 10.0
                : channel.RxMer;

            AppendPrometheusLine(sb,
                $"docsis_channel_rx_mer_db{{direction=\"{EscapeLabelValue("downstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\"}} {rxMer.ToString(CultureInfo.InvariantCulture)}");
        }
    }

    private static void AppendTotalCorrectedCodewordErrors(StringBuilder sb, Downstream downstream)
    {
        AppendPrometheusLine(sb, "# HELP docsis_channel_corrected_errors_total Total corrected codeword errors.");
        AppendPrometheusLine(sb, "# TYPE docsis_channel_corrected_errors_total counter");

        foreach (var channel in downstream.Channels)
        {
            AppendPrometheusLine(sb,
                $"docsis_channel_corrected_errors_total{{direction=\"{EscapeLabelValue("downstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\"}} {channel.CorrectedErrors}");
        }
    }

    private static void AppendTotalUncorrectedCodewordErrors(StringBuilder sb, Downstream downstream)
    {
        AppendPrometheusLine(sb, "# HELP docsis_channel_uncorrected_errors_total Total uncorrected codeword errors.");
        AppendPrometheusLine(sb, "# TYPE docsis_channel_uncorrected_errors_total counter");

        foreach (var channel in downstream.Channels)
        {
            AppendPrometheusLine(sb,
                $"docsis_channel_uncorrected_errors_total{{direction=\"{EscapeLabelValue("downstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\"}} {channel.UncorrectedErrors}");
        }
    }

    private static void AppendT3TimeoutsTotal(StringBuilder sb, Upstream upstream)
    {
        AppendPrometheusLine(sb, "# HELP docsis_channel_t3_timeouts_total Total T3 timeouts.");
        AppendPrometheusLine(sb, "# TYPE docsis_channel_t3_timeouts_total counter");

        foreach (var channel in upstream.Channels)
        {
            AppendPrometheusLine(sb,
                $"docsis_channel_t3_timeouts_total{{direction=\"{EscapeLabelValue("upstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\"}} {channel.T3Timeout}");
        }
    }

    private static void AppendT4TimeoutsTotal(StringBuilder sb, Upstream upstream)
    {
        AppendPrometheusLine(sb, "# HELP docsis_channel_t4_timeouts_total Total T4 timeouts.");
        AppendPrometheusLine(sb, "# TYPE docsis_channel_t4_timeouts_total counter");

        foreach (var channel in upstream.Channels)
        {
            AppendPrometheusLine(sb,
                $"docsis_channel_t4_timeouts_total{{direction=\"{EscapeLabelValue("upstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\"}} {channel.T4Timeout}");
        }
    }

    private static void AppendChannelMetadata(StringBuilder sb, Upstream upstream, Downstream downstream)
    {
        AppendPrometheusLine(sb, "# HELP docsis_channel_info Channel metadata.");
        AppendPrometheusLine(sb, "# TYPE docsis_channel_info gauge");

        foreach (var channel in downstream.Channels)
        {
            AppendPrometheusLine(sb,
                channel.FftType is not null
                    ? $"docsis_channel_info{{direction=\"{EscapeLabelValue("downstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\",modulation=\"{channel.Modulation}\",fft_type=\"{channel.FftType}\"}} 1"
                    : $"docsis_channel_info{{direction=\"{EscapeLabelValue("downstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\",modulation=\"{channel.Modulation}\"}} 1");
        }

        foreach (var channel in upstream.Channels)
        {
            AppendPrometheusLine(sb,
                channel.FftType is not null
                    ? $"docsis_channel_info{{direction=\"{EscapeLabelValue("upstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\",modulation=\"{channel.Modulation}\",fft_type=\"{channel.FftType}\"}} 1"
                    : $"docsis_channel_info{{direction=\"{EscapeLabelValue("upstream")}\",channel_id=\"{channel.ChannelId}\",channel_type=\"{channel.ChannelType}\",modulation=\"{channel.Modulation}\"}} 1");
        }
    }

    private static void AppendPrometheusLine(StringBuilder sb, string line) =>
        sb.Append(line).Append('\n');

    private static string EscapeLabelValue(string? value) =>
        (value ?? string.Empty)
        .Replace("\\", "\\\\", StringComparison.Ordinal)
        .Replace("\n", "\\n", StringComparison.Ordinal)
        .Replace("\"", "\\\"", StringComparison.Ordinal);
    
    
}