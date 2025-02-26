using AntiFraud.Core.Interfaces;
using AntiFraud.Core.Models;
using Confluent.Kafka;
using System.Text.Json;

namespace AntiFraud.Core.Service
{
    public class AntiFraudService : IAntiFraudService
    {
        private readonly ConsumerConfig config;
        private readonly IProducer<string, string> _producer;

        public AntiFraudService()
        {
            config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnablePartitionEof = true
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task<TransactionResponse> CheckTransactions()
        {
            try
            {
                string result = "";
                var transactions = new List<Transaction>();

                using (var consumer = new ConsumerBuilder<string, string>(config).Build())
                {
                    consumer.Subscribe("transaction-pending-events");

                    bool continueConsuming = true;

                    while (continueConsuming)
                    {
                        var consumeResult = consumer.Consume(CancellationToken.None);
                        if (consumeResult.IsPartitionEOF)
                        {
                            continueConsuming = false; // Exit the loop if no more messages are available
                            continue;
                        }
                        result = consumeResult.Message.Value;

                        var deliveryReport = new DeliveryResult<string, string>();
                        var transaction = JsonSerializer.Deserialize<Transaction>(result);

                        if (transaction.Value > 2000)
                        {
                            transaction.Status = 2; // set status to rejected
                        }

                        transaction.Status = 3; // set status to approved

                        var jsonMessage = JsonSerializer.Serialize(transaction);

                        var message = new Message<string, string>
                        {
                            Key = $"key-{transaction.Id}",
                            Value = jsonMessage
                        };

                        deliveryReport = await _producer.ProduceAsync("transaction-processed-events", message);
                        if (deliveryReport.Status != PersistenceStatus.Persisted)
                        {
                            return new TransactionResponse
                            {
                                IsSuccess = false,
                                MessageError = "Unable to create record in message broker"
                            };

                        }
                    }

                    return new TransactionResponse
                    {
                        IsSuccess = true
                    };
                }
            }

            catch (ConsumeException e)
            {
                return new TransactionResponse
                {
                    IsSuccess = false,
                    MessageError = e.Message
                };
            }
        }
    }


}


