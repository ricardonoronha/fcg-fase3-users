using System.Text;
using RabbitMQ.Client;
using System.Text.Json;

namespace FIAP.MicroService.Usuario.Infraestrutura.Services
{
    public class RabbitMQService
    {
        private readonly IConnection _connection;

        public RabbitMQService(IConnection connection)
        {
            this._connection = connection;
        }

        public async Task EnviarDados(object dados)
        {
            using var channel = await _connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(exchange: "user_exchange", type: ExchangeType.Fanout);

            var json = JsonSerializer.Serialize(dados);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(exchange: "user_exchange", routingKey: "", body: body);

            Console.WriteLine("[X] - TESTE CONEXAO RABBIT!");
        }
    }
}