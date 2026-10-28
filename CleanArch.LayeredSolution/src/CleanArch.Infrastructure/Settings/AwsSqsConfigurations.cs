namespace CleanArch.Infrastructure.Settings;

using System.Diagnostics.CodeAnalysis;
using Amazon;
using Microsoft.Extensions.Configuration;
using Wolverine;
using Wolverine.AmazonSqs;

[ExcludeFromCodeCoverage]
public static class AwsSqsConfigurations
{
    public static void ConfigureAwsSqs(
        this WolverineOptions options,
        IConfiguration configuration)
    {
        var region = configuration["AwsSettings:Region"];
        var queueName = configuration["AwsSettings:SqsQueueName"];
        if (string.IsNullOrWhiteSpace(queueName))
        {
            throw new InvalidOperationException("AwsSettings:SqsQueueName is required.");
        }

        options.UseAmazonSqsTransport(sqs =>
        {
            if (!string.IsNullOrWhiteSpace(region))
            {
                sqs.RegionEndpoint = RegionEndpoint.GetBySystemName(region);
            }
        });

        options.ListenToSqsQueue(queueName).ProcessInline();
    }
}