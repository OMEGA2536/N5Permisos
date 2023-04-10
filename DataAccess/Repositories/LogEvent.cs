using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using N5PermisosAPI.DataAccess.Interfaces;
using Nest;
using System.Net;
using System.Text.Json;

namespace N5PermisosAPI.DataAccess.Repositories
{
    public class LogEvent : ILogEvent
    {
        private readonly ILogger<LogEvent> _logger;
        private readonly IConfiguration Configuration;
        private readonly IElasticClient _elasticClient;
        private readonly ProducerConfig _producerConfig;
        public LogEvent(ILogger<LogEvent> logger, IConfiguration configuration, IElasticClient elasticClient, ProducerConfig producerConfig)
        {
            _logger = logger;
            Configuration = configuration;
            _elasticClient = elasticClient;
            _producerConfig = producerConfig;
        }
        async Task ILogEvent.LogEventToElasticsearchAsync(string eventType, object eventData)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(Configuration["Elasticsearch:Uri"]);

                    // Verifica la disponibilidad del Elasticsearch
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var eventLog = new
                        {
                            EventType = eventType,
                            EventData = eventData,
                            Timestamp = DateTime.UtcNow
                        };

                        await _elasticClient.IndexDocumentAsync(eventLog);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Elasticsearch no disponible");
            }
           
        }

        async Task ILogEvent.LogEventToKafkaAsync(string eventType, object eventData)
        {
            try
            {
                var config = new AdminClientConfig { BootstrapServers = Configuration["Kafka:BootstrapServers"] };
                using (var adminClient = new AdminClientBuilder(config).Build())
                {
                    // Verifica la disponibilidad del clúster de Kafka
                    var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));
                    if (metadata.Topics.Count == 0)
                    {
                        // El clúster de Kafka no está disponible, maneja el error
                        throw new Exception("El clúster de Kafka no está disponible.");
                    }
                }

                using (var producer = new ProducerBuilder<Null, string>(new ProducerConfig
                {
                    BootstrapServers = Configuration["Kafka:BootstrapServers"]
                }).Build())
                {
                    var eventLog = new
                    {
                        Id = Guid.NewGuid(),
                        OperationName = eventType,
                        EventData = eventData,
                        Timestamp = DateTime.UtcNow
                    };

                    var message = new Message<Null, string>
                    {
                        Value = JsonSerializer.Serialize(eventLog)
                    };

                    await producer.ProduceAsync(eventType, message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kafka no disponible");
            }
          
        }
    }
}
