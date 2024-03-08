using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using PlatformServiceMicroserver.DTOs;
using RabbitMQ.Client;
using System;
using System.Reflection;
using System.Text;
using System.Text.Json;


namespace PlatformServiceMicroserver.AsyncDataServices
{
	public class MessageBusClient : IMessageBusClient
	{
		private readonly IConfiguration _config;
		private readonly IConnection _connection;
		private readonly IModel _channel;


		public MessageBusClient(IConfiguration config)
        {
			_config = config;
			var factory = new ConnectionFactory()
			{
				HostName = _config["RabbitMQHost"],
				Port = int.Parse(_config["RabbitMQPort"])
			};
			try
			{
				_connection = factory.CreateConnection();
				_channel = _connection.CreateModel();
				_channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
				_connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;
				Console.WriteLine($"connected to message bus");

			}

			catch (Exception ex)
			{
				Console.WriteLine($"could not connect to the message bus:  {ex.Message}");
			}
		}


        public void PublishNewPlatform(PlatformPublishedDto platformPublished)
		{
			var message = JsonSerializer.Serialize(platformPublished);

			if (_connection.IsOpen)
            {
				Console.WriteLine("Rabbit connection Open, sending message.....");
				SendMessage(message);
				//to do send the message
			}
			else
			{
				Console.WriteLine($"rabbit connection closed, not sending");
			}
		}

		private void SendMessage(string message)
		{
			var body = Encoding.UTF8.GetBytes(message);

			_channel.BasicPublish(exchange: "trigger",
				routingKey: "",
				basicProperties: null,
				body: body);
			Console.WriteLine($" we have sent {message}");
		}

		public void Dispose()
		{
			Console.WriteLine($"MessageBus Dispose");
            if (_channel.IsOpen)
            {
				_channel.Close();
				_connection.Close();
            }
        }

		private void RabbitMQ_ConnectionShutDown(object sender, ShutdownEventArgs e)
		{
			Console.WriteLine($"Rabbit MQ connection shutDown.............");
			//_connection.Close();
		}

	}
}
