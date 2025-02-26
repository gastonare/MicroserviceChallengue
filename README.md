# Yape Challengue

This project consists of 2 microservices:

- Transaction: It's responsible for creating a transaction.
- Antifraud: It's responsible for processing the created transaction, which can be either approved or rejected.

## Flow

1) By hitting the https://localhost:7190/Transaction (POST) endpoint the transaction is being recorded in a database + in a "transaction-pending-events" (kafka queue). At this moment the transaction is being recoded as "pending"

2) Then you need to call to the https://localhost:7245/AntiFraud (POST) endpoint. This service is being in charge of process the remaining transactions that needs to be processed (pending status) in order to perform this action it consumes the "transaction-pending-events"(kafka queue) . Once that are processed are stored in the "transaction-processed-events" queue.

3) Finally we need to store this "result values" on the database. In order to perform this task we need to hit the https://localhost:7190/Transaction (PUT). This endpoint is being charged of consume the "processed-events" queue and then store the result on the database. Basically if it's a approved or rejected transaction.

## Architecture

It was based on the "onion" pattern:

```
/Transaction (same as AntiFraud project)
  ├── /Core
  │    ├── Models
  │    │     └── Transaction.cs
  │    ├── Interfaces
  │    │     └── ITransactionRepository.cs
  │    │     └── ITransactionService.cs
  │    └── Services
  │          └── TransactionService.cs
  ├── /Infrastructure
  │    └── Repositories
  │         └── TransactionRepository.cs
  ├── /Api
  │    ├── Controllers
  │    │    └── TransactionController.cs

```
