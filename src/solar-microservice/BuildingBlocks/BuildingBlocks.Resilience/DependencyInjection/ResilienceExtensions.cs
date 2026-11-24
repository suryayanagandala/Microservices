using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace BuildingBlocks.Resilience.DependencyInjection;

public static class ResilienceExtensions
{
    public static void AddSolarStandardResilience(this IHttpClientBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddStandardResilienceHandler(options =>
        {
            options.Retry.MaxRetryAttempts = 2;
            options.Retry.Delay = TimeSpan.FromMilliseconds(250);
            options.Retry.BackoffType = DelayBackoffType.Exponential;
            options.Retry.UseJitter = true;
            // Only retry safe reads. Cart/Orders use POST to fetch product batches;
            // retrying those requests could duplicate processing.
            options.Retry.ShouldHandle = args => ValueTask.FromResult(
                args.Outcome.Result?.RequestMessage?.Method == HttpMethod.Get
                && HttpClientResiliencePredicates.IsTransient(args.Outcome));

            options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(10);
            options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(3);

            options.CircuitBreaker.FailureRatio = 0.5;
            options.CircuitBreaker.MinimumThroughput = 5;
            options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
            options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(10);
        });
    }
}
