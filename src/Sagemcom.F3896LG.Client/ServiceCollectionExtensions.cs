using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace Sagemcom.F3896LG.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddModemClient(this IServiceCollection services,
        Action<ModemClientConfiguration> configure)
    {
        services.Configure(configure);

        services
            .AddRefitGeneratedClient<IModemClient>(new RefitSettings())
            .ConfigureHttpClient((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<ModemClientConfiguration>>();
                client.BaseAddress = options.Value.BaseAddress;
            }).ConfigurePrimaryHttpMessageHandler(sp =>
            {
                var options = sp.GetRequiredService<IOptions<ModemClientConfiguration>>().Value;

                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        options.TrustAllCertificates
                            ? HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                            : null
                };
            });

        return services;
    }
}