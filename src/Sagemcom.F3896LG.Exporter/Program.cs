using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Sagemcom.F3896LG.Client;
using Sagemcom.F3896LG.Exporter;

var builder = WebApplication.CreateSlimBuilder();

builder.WebHost.UseUrls("http://127.0.0.1:9090");

builder.Services.AddModemClient(options =>
{
    options.BaseAddress = new Uri("https://192.168.100.1");
    options.TrustAllCertificates = true;
});

var app = builder.Build();

app.MapGet("/metrics", async (IModemClient client) =>
{
    var downstream = await client.GetDownstreamStats();
    var upstream = await client.GetUpstreamStats();
    var rendered = PrometheusMetricsFormatter.Render(downstream, upstream);
    return Results.Text(rendered, "text/plain; version=0.0.4; charset=utf-8");
});

await app.RunAsync();