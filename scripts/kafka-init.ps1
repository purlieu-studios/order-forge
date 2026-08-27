$topics = @(
    "orderforge.orders.events"
    "orderforge.inventory.events"
    "orderforge.payments.events"
)

# Create each topic inside the already-running Kafka container.
foreach ($topic in $topics) {
    # -T prevents Docker from opening an interactive terminal.
    # --bootstrap-server selects the Kafka server.
    # --if-not-exists makes the command safe to run repeatedly.
    # Each topic gets three partitions and one copy because we have one Kafka server.
    docker compose exec -T kafka /opt/kafka/bin/kafka-topics.sh --bootstrap-server localhost:9092 --create --if-not-exists --topic $topic --partitions 3 --replication-factor 1

    # Stop immediately if Kafka reports that topic creation failed.
    if ($LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }
}