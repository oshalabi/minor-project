## ARD 2: Gebruik van Entity Framework core

| Name              | EF Core als ORM                                                   |
|-------------------|-----------------------------------------------------------|
| Current version   | 1                                                         |
| Current status    | 🟩 **DECIDED**                                            |
| Problem/Issue     | Wij willen een "code first" database. Dit is het beste te doen met een ORM. Hiervoor zijn performance en gebruikersgemak van belang.|
| Decision          | Gekozen voor EF core.|
| Alternatives      | 1.  ADO.NET: Directe toegang tot de database via SQL-queries, maar vereist dat sql queries geschreven worden en biedt minder voordelen in termen van productiviteit en onderhoudbaarheid. </br> 2. Dapper: Een lichte en snelle micro-ORM voor eenvoudige queries, maar biedt geen volledige ondersteuning voor features zoals lazy loading, change tracking, of complexe object-relaties. |
| Arguments         | 1. Volledige ORM-functionaliteit: EF Core biedt een simpele en flexibele oplossing voor het omgaan met object-relaties, database-schema's en migraties. </br> 2. Migraties en schema-beheer: De ingebouwde ondersteuning voor database-migraties maakt het eenvoudig om schemawijzigingen door te voeren zonder handmatig SQL-scripts te schrijven. </br> 3. Ondersteuning voor LINQ: EF Core biedt krachtige ondersteuning voor LINQ, wat het mogelijk maakt om queries te schrijven op een manier die natuurlijk aansluit bij object-georiënteerd programmeren. </br> 4. Grote community en ondersteuning: EF Core wordt actief onderhouden door Microsoft en heeft een grote en actieve community, wat betekent dat het een goed gedocumenteerde en ondersteunde technologie is. </br> 5. Cross-platform: EF Core is platformonafhankelijk en ondersteunt niet alleen .NET Framework maar ook .NET Core, waardoor het geschikt is voor zowel webtoepassingen als microservices. |
