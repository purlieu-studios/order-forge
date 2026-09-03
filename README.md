# OrderForge

OrderForge is a learning-focused order-processing system designed to demonstrate how a browser request moves through independently deployable services, databases, and messaging systems.

## End goal

The React client sends requests to the .NET Gateway. The Gateway is the authenticated public HTTP boundary and forwards order requests to Orders and stock-management requests to Inventory. Orders owns the order lifecycle and its data. Inventory and Payments each own their own data and process the order workflow independently. A correlation ID follows the order through HTTP requests, messages, logs, and traces so the entire flow can be observed.

## End-to-end flow

```mermaid
sequenceDiagram
    actor Client as React client
    participant Entra as Microsoft Entra ID
    participant Gateway as .NET Gateway
    participant Orders as Orders service
    participant OrdersDb as Orders database
    participant Rabbit as RabbitMQ
    participant Inventory as Inventory service
    participant InventoryDb as Inventory database
    participant Payments as Payments service
    participant PaymentsDb as Payments database
    participant Bus as Azure Service Bus
    participant Functions as Azure Functions
    participant Logic as Azure Logic Apps
    participant Kafka as Apache Kafka

    Client->>Entra: Sign in
    Entra-->>Client: Access token
    Note over Client,Gateway: Requests include the bearer token and correlation ID

    Client->>Gateway: Stock-management request
    Gateway->>Inventory: HTTP stock operation
    Inventory->>InventoryDb: Update stock
    Inventory-->>Gateway: Stock result
    Gateway-->>Client: Stock result

    Client->>Gateway: Submit order
    Gateway->>Orders: Create order
    Orders->>OrdersDb: Save order and outbox message
    Orders-->>Gateway: Initial order status
    Gateway-->>Client: Initial status and correlation ID

    Orders->>Rabbit: Publish workflow command
    par Inventory reservation
        Rabbit->>Inventory: Reserve stock
        Inventory->>InventoryDb: Update reservation
        Inventory-->>Rabbit: Reservation result
    and Payment authorization
        Rabbit->>Payments: Authorize payment
        Payments->>PaymentsDb: Record authorization
        Payments-->>Rabbit: Payment result
    end

    Rabbit-->>Orders: Workflow results
    Orders->>OrdersDb: Record final outcome
    Orders->>Bus: Publish integration event
    Bus-->>Functions: Reconciliation, expiry, and projections
    Bus-->>Logic: Customer and operations notifications
    Orders-->>Kafka: Domain event
    Inventory-->>Kafka: Domain event
    Payments-->>Kafka: Domain event
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
