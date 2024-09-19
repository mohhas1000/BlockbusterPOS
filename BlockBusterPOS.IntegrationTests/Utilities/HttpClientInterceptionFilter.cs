using JustEat.HttpClientInterception;
using Microsoft.Extensions.Http;
using System.Diagnostics.CodeAnalysis;

namespace BlockBusterPOS.IntegrationTests.Utilities;

[ExcludeFromCodeCoverage]
internal sealed class HttpClientInterceptionFilter : IHttpMessageHandlerBuilderFilter
{
    private readonly HttpClientInterceptorOptions _options;

    internal HttpClientInterceptionFilter(HttpClientInterceptorOptions options)
    {
        _options = options;
    }

    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
    {
        return (builder) =>
        {
            next(builder);

            builder.AdditionalHandlers.Add(_options.CreateHttpMessageHandler());
        };
    }
}

