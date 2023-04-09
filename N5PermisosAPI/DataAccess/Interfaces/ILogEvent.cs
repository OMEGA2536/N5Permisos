namespace N5PermisosAPI.DataAccess.Interfaces
{
    public interface ILogEvent
    {
        Task LogEventToElasticsearchAsync(string eventType, object eventData);
        Task LogEventToKafkaAsync(string eventType, object eventData);
    }
}
