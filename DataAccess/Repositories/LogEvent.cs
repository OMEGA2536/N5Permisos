using Confluent.Kafka;
using N5PermisosAPI.DataAccess.Interfaces;
using Nest;
using System.Text.Json;

namespace N5PermisosAPI.DataAccess.Repositories
{
    public class LogEvent : ILogEvent
    {
        private readonly IElasticClient _elasticClient;
        private readonly ProducerConfig _producerConfig;
        public LogEvent(IElasticClient elasticClient, ProducerConfig producerConfig)
        {
            _elasticClient = elasticClient;
            _producerConfig = producerConfig;
        }
        async Task ILogEvent.LogEventToElasticsearchAsync(string eventType, object eventData)
        {
            var eventLog = new
            {
                EventType = eventType,
                EventData = eventData,
                Timestamp = DateTime.UtcNow
            };

            await _elasticClient.IndexDocumentAsync(eventLog);
        }

        async Task ILogEvent.LogEventToKafkaAsync(string eventType, object eventData)
        {
            using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {
                var eventLog = new
                {
                    EventType = eventType,
                    EventData = eventData,
                    Timestamp = DateTime.UtcNow
                };

                var message = new Message<Null, string>
                {
                    Value = JsonSerializer.Serialize(eventLog)
                };

                await producer.ProduceAsync("event_log", message);
            }
        }
    }
}
