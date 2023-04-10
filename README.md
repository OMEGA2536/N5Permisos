# N5Permisos
#appsetting
{
  "ConnectionStrings": {
    "N5PermisosAPIContext": "Server=WATUNCAR\\OMEGANET_2020;Database=N5-Challenge;User Id=sa; Password=123456; TrustServerCertificate=True;"
  },
  "Elasticsearch": {
    "Uri": "http://localhost:9200"
  },
  "Kafka": {
    "BootstrapServers": "localhost:9092"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
#docker
docker build -t n5dockerapi -f Dockerfile .
#test
N5PermisosAPI.Tests
  Pruebas en grupo:3
  Duración total:76 ms

Salidas
  3Correcta