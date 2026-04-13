## ADR: Implementatie van RabbitMQ als Message Broker
 
| **Name**              | Implementatie van RabbitMQ als Message Broker                                  |
|------------------------|-------------------------------------------------------------------------------|
| **Current version**    | 1                                                                             |
| **Current status**     | 🟩 **DECIDED**                                                                |
| **Problem/Issue**      | In een microservices-architectuur is er behoefte aan betrouwbare en schaalbare asynchrone communicatie. De huidige directe communicatie via HTTP creëert strakke koppelingen tussen services, wat de schaalbaarheid en fouttolerantie belemmert. Een message broker is nodig om decoupling te ondersteunen en berichtenbetrouwbaarheid te garanderen. |
 
---
 
| **Decision**           | RabbitMQ                                                                       |
|------------------------|-------------------------------------------------------------------------------|
| **Alternatives**       | 1. **Apache Kafka**: Een schaalbaar event-streamingplatform, maar complexer voor simpele message brokering.<br>2. **Redis Pub/Sub**: Simpel en snel, maar zonder berichtpersistentie of foutafhandelingsmechanismen.<br>3. **Amazon SQS**: Gehoste service met minimale setup, maar afhankelijkheid van AWS en kosten per bericht. |
| **Arguments**          | 1. RabbitMQ biedt uitstekende ondersteuning voor complexe berichtpatronen zoals publish/subscribe, fan-out en topic routing.<br>2. Het ondersteunt message durability en acknowledgements om berichtenverlies te voorkomen.<br>3. RabbitMQ heeft een robuuste webinterface voor monitoring en beheer.<br>4. Het is volledig open-source, wat kosten beperkt en aanpasbaarheid vergroot.<br>5. Brede ondersteuning voor integraties met veel programmeertalen en frameworks. |
 
---
 
| **Stakeholder**        | **Action**           | **Status**        | **Date**        |
|------------------------|----------------------|-------------------|-----------------|
| Joris Wittenberg       | Decision             | 🟩 **DECIDED**    | 13-12-2024      |
 
 
 
 
 
### Implementatiedetails
 
| **Aspect**             | **Details**                                                                  |
|------------------------|-------------------------------------------------------------------------------|
| **Exchange type**       | Direct exchanges voor gerichte communicatie en fan-out exchanges voor broadcast. |
| **Queues**              | Elke service heeft een eigen queue om berichten te verwerken.               |
| **Retry mechanism**     | Berichten met een TTL worden automatisch naar een Dead Letter Queue (DLQ) gestuurd bij falen. |
| **Clustering**          | RabbitMQ wordt uitgevoerd in een cluster voor hoge beschikbaarheid en schaalbaarheid. |
 
 
### Gevolgen
 
| **Type**               | **Details**                                                                  |
|------------------------|-------------------------------------------------------------------------------|
| **Voordelen**          | - Vermindert afhankelijkheid tussen services.<br>- Ondersteunt schaalbare, asynchrone communicatie.<br>- Biedt robuuste foutafhandeling en monitoring. |
| **Nadelen**            | - Vereist extra infrastructuur en onderhoud.<br>- Mogelijke leercurve voor teams die RabbitMQ nog niet kennen. |