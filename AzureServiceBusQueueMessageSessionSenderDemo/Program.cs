using Azure.Messaging.ServiceBus;
using MessageBrokerExplorationPOCLibrary;
using System.Text;
using System.Text.Json;


//Created on my local machine user environment variable
string serviceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
string queueName = Environment.GetEnvironmentVariable("MarketInsightQueueName");


// Create a ServiceBusClient
ServiceBusClient client = new ServiceBusClient(serviceBusConnectionString);

// Create a sender for the queue
ServiceBusSender sender = client.CreateSender(queueName);

string sessionId = "trade1session";


var trade1 = new Trade(1, 1, "BTC/USD", 40000.0, 39000.0, 42000.0);
var trade2 = new Trade(2, 2, "ETH/USD", 3000.0, 2900.0, 3200.0);
var trade3 = new Trade(3, 3, "XRP/USD", 1.0, 0.9, 1.2);

// Send each Trade object as a separate message with the same Session ID
await SendTradeMessage(sender, trade1, sessionId);
await SendTradeMessage(sender, trade2, sessionId);
await SendTradeMessage(sender, trade3, sessionId);

Console.WriteLine("All trade messages sent to the queue with the same session ID.");


async Task SendTradeMessage(ServiceBusSender sender, Trade trade, string sessionId)
{
    // Serialize the trade object to JSON
    string json = JsonSerializer.Serialize(trade);

    // Create a message to send with a session ID
    ServiceBusMessage message = new ServiceBusMessage(Encoding.UTF8.GetBytes(json))
    {
        SessionId = sessionId
    };

    // Send the message
    await sender.SendMessageAsync(message);
}

