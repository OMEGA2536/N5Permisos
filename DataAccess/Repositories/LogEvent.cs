using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using N5PermisosAPI.DataAccess.Interfaces;
using Nest;
using System.Net;
using System.Text.Json;

namespace N5PermisosAPI.DataAccess.Repositories
{
    public class LogEvent : ILogEvent
    {
        private readonly IConfiguration Configuration;
        private readonly IElasticClient _elasticClient;
        private readonly ProducerConfig _producerConfig;
        public LogEvent(IConfiguration configuration, IElasticClient elasticClient, ProducerConfig producerConfig)
        {
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

                
            }
           
        }

        async Task ILogEvent.LogEventToKafkaAsync(string eventType, object eventData)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(Configuration["Kafka:BootstrapServers"]);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
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
                }
            }
            catch (Exception)
            {

                
            }
          
        }
    }
}
