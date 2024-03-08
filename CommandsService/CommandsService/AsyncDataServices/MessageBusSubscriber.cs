﻿using CommandsService.EventProcessing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommandsService.AsyncDataServices
{
	public class MessageBusSubscriber : BackgroundService
	{
		private readonly IConfiguration _config;
		private readonly IEventProcessor _eventProcessor;
		private IConnection _connection;
		private IModel _channel;
		private string _queueName;

		public MessageBusSubscriber(IConfiguration config, IEventProcessor eventProcessor)
		{
			_config = config;
			_eventProcessor = eventProcessor;

			InitializeRabbitMQ();
		}



		private void InitializeRabbitMQ()
		{
			var factory = new ConnectionFactory()
			{
				HostName = _config["RabbitMQHost"],
				Port = int.Parse(_config["RabbitMQPort"])
			};

			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
			_queueName = _channel.QueueDeclare().QueueName;
			_channel.QueueBind(queue: _queueName,
				exchange: "trigger",
				routingKey: ""
				);
			Console.WriteLine("Listening on the Message Bus......");
			_connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;

		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer( _channel);

			consumer.Received += (ModuleHandle, ea) =>
			{
				Console.WriteLine("Event received........");
				var body = ea.Body;
				var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

				_eventProcessor.ProcessEvent(notificationMessage);
			};

			_channel.BasicConsume(
				queue: _queueName,
				autoAck: true, 
				consumer: consumer
				);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			Console.WriteLine($"MessageBus Dispose");
			if (_channel.IsOpen)
			{
				_channel.Close();
				_connection.Close();
			}
			base.Dispose();
		}

		private void RabbitMQ_ConnectionShutDown(object sender, ShutdownEventArgs e)
		{
			Console.WriteLine($"Rabbit MQ connection shutDown.......");
			//_connection.Close();
		}
	}
}
