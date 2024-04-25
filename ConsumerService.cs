using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Kafka.Consumer.Console
{
    public class ConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ConsumerConfig _consumerConfig;
        private readonly ILogger<ConsumerService> _logger;
        private readonly ParametersModel _parameters;

        public ConsumerService(ILogger<ConsumerService> logger)
        {
            _parameters = new ParametersModel();
            _logger = logger;
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _parameters.BootstrapServer,
                GroupId = _parameters.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest // nenhum consumidor vai pegar novamente a mensagem
            };
            _consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var mensagem = "";
            _logger.LogInformation("Aguardando mensagens...");
            _consumer.Subscribe(_parameters.Topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Run(() =>
                    {
                        var consumeResult = _consumer.Consume(stoppingToken);
                        var template = JsonSerializer.Deserialize<TemplateResponse>(consumeResult.Message.Value);

                        mensagem = "GroupId:" + _parameters.GroupId + "\n" +
                                   "Mensagem lida: " + "De: " + template.De + " Para: " + template.Para + " Assunto: " + template.Assunto;
                        _logger.LogInformation(mensagem);
                    }, stoppingToken);
                }
                catch (OperationCanceledException e)
                {
                    mensagem = "Operação cancelada.";
                    _logger.LogInformation(e, mensagem);
                }
                catch (ConsumeException e)
                {
                    mensagem = $"Erro ao consumir mensagem: {e.Error.Reason}";
                    _logger.LogError(e, mensagem);
                }
            }
            _consumer.Consume(stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Parando o consumidor...");
            _consumer.Close();
            _consumer.Dispose();
            return Task.CompletedTask;
        }
    }
}
