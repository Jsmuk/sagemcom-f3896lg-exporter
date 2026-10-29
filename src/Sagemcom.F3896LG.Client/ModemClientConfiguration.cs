namespace Sagemcom.F3896LG.Client;

public sealed class ModemClientConfiguration
{
    public required Uri BaseAddress { get; set; }
    public required bool TrustAllCertificates { get; set; }
}