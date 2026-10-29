using Refit;
using Sagemcom.F3896LG.Client.Models;

namespace Sagemcom.F3896LG.Client;

public interface IModemClient
{
    [Get("/rest/v1/cablemodem/downstream")]
    Task<DownstreamResponse> GetDownstreamStats();
    [Get("/rest/v1/cablemodem/upstream")]
    Task<UpstreamResponse> GetUpstreamStats();
}