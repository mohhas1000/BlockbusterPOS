using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using JustEat.HttpClientInterception;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace BlockBusterPOS.IntegrationTests.Utilities
{
    [ExcludeFromCodeCoverage]
    public class TestWebApplicationBuilder<TEntryPoint> : WebApplicationFactory<TEntryPoint>, IDisposable where TEntryPoint : class
    {
        private readonly string _configFiles;
        private readonly string _environment = "Development";
        private bool _preserveExecutionContext = false;
        private bool _isDisposed = false;

        private readonly Collection<IDisposable> _disposables = new();
        private Action<IServiceCollection>? _servicesConfiguration;

        public HttpClientInterceptorOptions HttpClientInterceptorOptions { get; }

        public TestWebApplicationBuilder(string jsonConfigFile)
        {
            _configFiles = jsonConfigFile;
            HttpClientInterceptorOptions = new HttpClientInterceptorOptions() { ThrowOnMissingRegistration = true };
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IHttpMessageHandlerBuilderFilter, HttpClientInterceptionFilter>(
                    _ => new HttpClientInterceptionFilter(HttpClientInterceptorOptions));
                _servicesConfiguration?.Invoke(services);
            });

            builder.ConfigureHostConfiguration(config =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile(_configFiles);
                config.AddEnvironmentVariables();
            });

            return base.CreateHost(builder);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    foreach (var disposable in _disposables)
                    {
                        disposable.Dispose();
                    }
                    _disposables.Clear();
                }

                _isDisposed = true;
            }
        }

        internal HttpClient Build()
        {
            WebApplicationFactory<TEntryPoint> factory = GetWebApplicationFactory();
            return CreateHttpClient(factory);
        }

        internal TestWebApplicationBuilder<TEntryPoint> WithTestServices(Action<IServiceCollection> servicesConfiguration)
        {
            _servicesConfiguration = servicesConfiguration;
            return this;
        }

        private static HttpClient CreateHttpClient(WebApplicationFactory<TEntryPoint> factory)
        {
            HttpClient client = factory.CreateClient();
            return client;
        }

        private WebApplicationFactory<TEntryPoint> GetWebApplicationFactory()
        {
            var factory = new WebApplicationFactory<TEntryPoint>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        _servicesConfiguration?.Invoke(services);
                    });
                    builder.UseEnvironment(_environment);
                });

            factory.Server.PreserveExecutionContext = _preserveExecutionContext;

            return factory;
        }

        internal TestWebApplicationBuilder<TEntryPoint> WithPreserveExecutionContext(bool preserveExecutionContext)
        {
            _preserveExecutionContext = preserveExecutionContext;
            return this;
        }
    }
}
