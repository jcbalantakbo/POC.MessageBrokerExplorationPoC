using Azure.Messaging.ServiceBus;
using System.Text;
using MessageBrokerExplorationPOCLibrary;
using System.Text.Json;

//Created on my local machine user environment variable
string serviceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
string queueName = Environment.GetEnvironmentVariable("MarketInsightQueueName");
string sessionId = "trade1session";

await using (var client = new ServiceBusClient(serviceBusConnectionString))
{
    // Create a receiver for the session
    // Note that session must be enabled on the queue/topic that you are using before you can run this
    // and no you can't update a queue/topic to enable sessions.
    // You must create them and set the session to enabled
    ServiceBusSessionReceiver sessionReceiver = await client.AcceptSessionAsync(queueName, sessionId);

    // Continuously receive messages from the session
    while (true)
    {
        ServiceBusReceivedMessage receivedMessage = await sessionReceiver.ReceiveMessageAsync();

        if (receivedMessage != null)
        {

            string body = Encoding.UTF8.GetString(receivedMessage.Body);
            Trade trade = JsonSerializer.Deserialize<Trade>(body);
            Console.WriteLine($"MessageId: {receivedMessage.MessageId}");
            Console.WriteLine($"Subject: {receivedMessage.Subject}");
            Console.WriteLine($"TradeId: {trade.TradeId}");
            Console.WriteLine($"MarketType: {trade.MarketType}");
            Console.WriteLine($"Pair: {trade.Pair}");
            Console.WriteLine($"EntryPrice: {trade.EntryPrice}");
            Console.WriteLine($"StopLoss: {trade.StopLoss}");
            Console.WriteLine($"TakeProfit: {trade.TakeProfit}");
            Console.WriteLine();
            Console.WriteLine();
            // Complete the message so it's removed from the queue
            await sessionReceiver.CompleteMessageAsync(receivedMessage);
        }
        else
        {
            // No more messages in the session
            break;
        }
    }
}