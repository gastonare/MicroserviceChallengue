using Confluent.Kafka;
using System.Text.Json;
using Transaction.Core.Interfaces;
using Transaction.Core.Models;
using static Confluent.Kafka.ConfigPropertyNames;
using TransactionModel = Transaction.Core.Models.Transaction;

namespace Transaction.Core.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProducer<string, string> _producer;

        private readonly ConsumerConfig config;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnablePartitionEof = true
            };

            _transactionRepository = transactionRepository;
            _producer = new ProducerBuilder<string, string>(config).Build();
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionResponse> CreateTransaction(TransactionBody transactionBody)
        {
            try
            {
                var transaction = new TransactionModel
                {
                    SourceAccountId = transactionBody.SourceAccountId,
                    TargetAccountId = transactionBody.TargetAccountId,
                    TransactionExternalId = Guid.NewGuid(),
                    TransferTypeId = transactionBody.TransferTypeId,
                    CreatedAt = DateTime.UtcNow,
                    Status = 1, // pending
                    Value = transactionBody.Value
                };

                // insert transaction into the database
                var createdTransaction = _transactionRepository.InsertRecord(transaction);

                if (createdTransaction == null)
                {
                    return new TransactionResponse
                    {
                        IsSuccess = false,
                        MessageError = "Unable to insert record into the database",
                    };
                }

                var jsonSerializedValue = JsonSerializer.Serialize(transaction);
                var message = new Message<string, string>
                {
                    Key = $"key-{createdTransaction.Id}",
                    Value = jsonSerializedValue
                };

                // insert record into message broker
                var deliveryReport = await _producer.ProduceAsync("transaction-pending-events", message);

                if (deliveryReport.Status != PersistenceStatus.Persisted)
                {
                    return new TransactionResponse
                    {
                        IsSuccess = false,
                        MessageError = "Unable to create record in message broker"
                    };
                }

                return new TransactionResponse
                {
                    CreatedAt = createdTransaction.CreatedAt,
                    IsSuccess = true,
                    TransactionExternalId = createdTransaction.TransactionExternalId
                };

            }
            catch (Exception ex)
            {
                return new TransactionResponse
                {
                    IsSuccess = false,
                    MessageError = ex.Message
                };
            }
        }

        public TransactionResponse UpdateTransactions()
        {
            try
            {
                string result = null;

                using (var consumer = new ConsumerBuilder<string, string>(config).Build())
                {
                    consumer.Subscribe("transaction-processed-events");
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

                        var transaction = JsonSerializer.Deserialize<Models.Transaction>(result);

                        var message = new Message<string, string>
                        {
                            Key = $"key-{transaction.Id}",
                            Value = result
                        };

                        var updatedTransaction = _transactionRepository.UpdateRecord(transaction);
                        if (updatedTransaction == null)
                        {
                            return new TransactionResponse
                            {
                                IsSuccess = false,
                                MessageError = "Unable to update record into the database",
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


        //finally
        //{
        //    _consumer.Dispose();
        //    _producer.Dispose();
        //}
    }
}



