# OrderForge

OrderForge is a learning-focused order-processing system designed to demonstrate how a browser request moves through independently deployable services, databases, and messaging systems.

## End goal

The React client sends an order to the .NET Gateway. The Gateway is the public HTTP boundary and forwards the request to the Orders service. Orders owns the order lifecycle and its data. Inventory and Payments each own their own data and process the order workflow independently. A correlation ID follows the order through HTTP requests, messages, logs, and traces so the entire flow can be observed.

## Architecture

```mermaid
flowchart LR
    Client["React + TypeScript<br/>Browser"] -->|HTTP + correlation ID| Gateway[".NET Gateway<br/>Public boundary"]
    Gateway -->|HTTP| Orders["Orders service<br/>Lifecycle + orchestration"]
    Orders --> OrdersDb[("Orders database<br/>PostgreSQL / Azure SQL")]
    Orders -->|Outbox command| Rabbit["RabbitMQ<br/>Internal workflow"]
    Rabbit -->|Reserve stock| Inventory["Inventory service"]
    Rabbit -->|Authorize payment| Payments["Payments service"]
    Inventory --> InventoryDb[("Inventory database")]
    Payments --> PaymentsDb[("Payments database")]
    Inventory -->|Reservation result| Rabbit
    Payments -->|Payment result| Rabbit
    Rabbit -->|Workflow result| Orders
    Orders -->|Integration events| ServiceBus["Azure Service Bus"]
    ServiceBus --> Functions["Azure Functions"]
    ServiceBus --> LogicApps["Azure Logic Apps"]
    Orders -.->|Domain events| Kafka["Apache Kafka<br/>Activity • audit • analytics"]
    Inventory -.->|Domain events| Kafka
    Payments -.->|Domain events| Kafka

    classDef client fill:#e0ecff,stroke:#2563eb,color:#0f172a;
    classDef boundary fill:#dbeafe,stroke:#1d4ed8,color:#0f172a;
    classDef service fill:#dcfce7,stroke:#15803d,color:#0f172a;
    classDef database fill:#f1f5f9,stroke:#475569,color:#0f172a;
    classDef messaging fill:#ffedd5,stroke:#c2410c,color:#0f172a;
    classDef cloud fill:#cffafe,stroke:#0e7490,color:#0f172a;
    classDef stream fill:#ede9fe,stroke:#6d28d9,color:#0f172a;

    class Client client;
    class Gateway boundary;
    class Orders,Inventory,Payments service;
    class OrdersDb,InventoryDb,PaymentsDb database;
    class Rabbit messaging;
    class ServiceBus,Functions,LogicApps cloud;
    class Kafka stream;
```

The intended workflow is:

1. Orders stores the order and writes an outbox message in the same database transaction.
2. RabbitMQ carries internal workflow commands between Orders, Inventory, and Payments.
3. Inventory reserves or rejects stock, and Payments authorizes or rejects payment.
4. Orders records the final outcome and publishes integration events through Azure Service Bus.
5. Azure Functions consume those events for asynchronous work such as reconciliation, expiry, and projections.
6. Azure Logic Apps handle external workflows such as customer and operations notifications.
7. Kafka receives domain events for an independent activity stream, audit view, and analytics consumers.

## Technology stack

- React and TypeScript for the browser client
- .NET 10 for the Gateway, Orders, Inventory, Payments, and shared Contracts projects
- PostgreSQL locally, with Azure SQL as the cloud database target; each service owns a separate database
- RabbitMQ for internal application workflow
- Azure Service Bus for the durable Azure integration boundary
- Azure Functions for asynchronous event processing
- Azure Logic Apps for external business workflows and notifications
- Apache Kafka for durable event streaming, audit, and analytics
- Docker Compose for reproducible local infrastructure
- Kubernetes for learning deployments, service discovery, configuration, health probes, and scaling
- Semantic Kernel for a bounded Operations Assistant that can query operational data without replacing deterministic business workflows

The project is intended to showcase practical distributed-systems patterns, including transactional outbox messaging, idempotent consumers, retries, compensation, observability, and independently deployable services.
