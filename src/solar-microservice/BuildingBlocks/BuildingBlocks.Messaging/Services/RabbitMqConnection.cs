using BuildingBlocks.Messaging.Configuration;
using BuildingBlocks.Messaging.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Services
{
    public sealed class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly ConnectionFactory _factory;
        private readonly ILogger<RabbitMqConnection> _logger;
        private readonly SemaphoreSlim _connectionLock = new(1, 1);
        private IConnection? _connection;

        public RabbitMqConnection(
            IOptions<RabbitMqSettings> options,
            ILogger<RabbitMqConnection> logger)
        {
            _logger = logger;
            var settings = options.Value;

            _factory = new ConnectionFactory
            {
                HostName = settings.HostName,
                Port = settings.Port,
                UserName = settings.UserName,
                Password = settings.Password,
                VirtualHost = settings.VirtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
            };
        }

        public async Task<IConnection> GetConnectionAsync()
        {
            if (_connection is { IsOpen: true })
                return _connection;

            await _connectionLock.WaitAsync();

            try
            {
                if (_connection is { IsOpen: true })
                    return _connection;

                const int maxAttempts = 10;

                for (var attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    try
                    {
                        _connection = await _factory.CreateConnectionAsync();
                        return _connection;
                    }
                    catch (Exception ex) when (attempt < maxAttempts)
                    {
                        var delay = TimeSpan.FromSeconds(Math.Min(attempt, 5));

                        _logger.LogWarning(
                            ex,
                            "Unable to connect to RabbitMQ. Retrying in {DelaySeconds} seconds ({Attempt}/{MaxAttempts}).",
                            delay.TotalSeconds,
                            attempt,
                            maxAttempts);

                        await Task.Delay(delay);
                    }
                }

                throw new InvalidOperationException("Unable to connect to RabbitMQ.");
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
                await _connection.DisposeAsync();

            _connectionLock.Dispose();
        }
    }
}
