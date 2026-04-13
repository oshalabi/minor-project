# Eigen bijdrage Hugo Kemme


## 1. Code/platform bijdrage

Competenties: *DevOps-1 Continuous Delivery*

Dev:
- In onnze applicatie was een monoliet aan het opkomen in een microservice. Ik heb deze weer uit elkaar getrokken en ervoor gezorgd dat de microservices netjes opgesplit waren. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8046?_a=files).
- Ik heb de krachtvoertabel gemaakt. Hieronder vallen ook de crud operaties. Zie pull-requests ([create](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8199?_a=files), [get](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8170?_a=files&path=/src/WebApp/src/app/energy-food/energy-food.component.ts) & [backend](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8239?_a=files)).
- De livestock properties, zowel fronend en backend. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8612).


Ops:
- De normAPI pod wilde niet opstarten in het cluster. Ben er uiteindelijk achtergekomen dat de healthcheck ontbrak en deze toegevoegd. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8856?_a=files).
- Bijdrage aan kubernetes infrastructure bestanden. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/mcps-gitops/pushes?itemVersion=GBmain&userId=5e30f7c5-944f-66a2-88ef-9bf5f0a8d160&userName=Hugo%20Kemme%20(student)).
  
## 2. Bijdrage app configuratie/containers/kubernetes

Competenties: *DevOps-2 Orchestration, Containerization*

- Ik heb de deployment.yaml en service.yaml voor de BasalRationAPI geschreven. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/mcps-gitops/pushes/104669).
- Toevoegen van de ingress.yaml. Zie [push](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pushes/99766).
- Toevoegen van BasalRation microservice in de docker-compose met dockerfile. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7068?path=/src/docker-compose.yml).
- De applicatie starte vaak lokaal niet goed op in docker. Dit gebeurde omdat er bepaalde containers al opstartte voordat rabbitmq opgestart was. Ik heb hier healthchecks aan toegevoegd in de dockercompose. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8046?_a=files&path=/src/docker-compose.yml).

## 3. Bijdrage versiebeheer, CI/CD pipeline en/of monitoring

Competenties: *DevOps-1 - Continuous Delivery*, *DevOps-3 GitOps*, *DevOps-5 - SlackOps*

- Het maken van bouwen en pushen van images in de pipeline. Zie [push](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pushes/100096).
- Ik heb de pipeline en bijbehorende scripts voor het uitrollen van een nieuwe omgeving met zo min mogelijk handmatige operaties met Harutjun en Joris geschreven. Zie [commits](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/mcps-gitops/pushes?itemVersion=GBmain)


## 4. Onderzoek

Competenties: *Nieuwsgierige houding*

Onderzoek:
Ik heb nog niks aan mijn onderzochte devops tool kunnen doen. Ik heb wel veel onderzoek moeten doen naar hoe angular werkt (ookal past dit niet in het devops landschap).

Domein:
- Ik heb het domein opgesteld met het domeinmodel. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7017).
- In dit [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/commit/47041814a05ac4258867eb58613ccd2efe3348e7?refName=refs%2Fheads%2Ffix-domain) hebben wij gezamelijk het domein aangepast.
## 5. Bijdrage code review/kwaliteit anderen en security

Competenties: *DevOps-7 - Attitude*, *DevOps-4 DevSecOps*

Code review:
- [Review](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8621) op code.
- [Review](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8222) op code.
- [Review](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8041) op code.
## 6. Bijdrage documentatie

Competenties: *DevOps-6 Onderzoek*

- Zoals eerder al gemeld, heb ik het domeinmodel opgesteld.
- Ik heb [adr-2.md](../docs/adr/adr-2.md) over EF Core geschreven.
- Ik heb aan het C4-model gewerkt. Dit is uiteindelijk niet naar main gegaan, doordat er niet goed gecommuniceerd was en iemand anders het ook al gemaakt had. Zie [branch](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5?version=GBdocs-C4-model&path=/docs/c4-model-rantsoenwijzer.md).


## 7. Bijdrage Agile werken, groepsproces, communicatie opdrachtgever en soft skills

Competenties: *DevOps-1 - Continuous Delivery*, *Agile*

- Ik heb samen met Osama deelgenomen aan alle Q&A's. Zie [notules](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8949).
- Ik heb de sprint 2 sprint planning geleid.
- Ik heb de sprint 1 review gepresenteerd.
- Ik ben tijdens een aantal DSU's de scrum master geweest.
- Ik heb de opdrachtgever een update gestuurd en een lijstje functionaliteiten met prioriteiten. Hierbij de vraag of hij het goed vind zo.

## 8. Leerervaringen

Competenties: *DevOps-7 - Attitude*

Tops:
- Dit is het eerste project voor mij dat we vanuit huis maken. Uit mijn ervaring met stage had ik met thuiswerken moeite met concentreren. Ik heb dat tijdens dit project niet en besteed mijn tijd nuttig.
- Ik ben goed bereikbaar en meteen instaat om met iemand te bellen als die hulp nodig heeft.
- heb dit project heel veel nieuwe dingen geleerd.

Tips:
- Niet tot weinig gecommuniceerd met de opdrachtgever. Dit heeft geleid tot een sprint review waar de opdrachtgever niet over was te spreken en nogal was verbaast dat wij niks hebben laten weten. Dit hebben wij vervolgens aangepakt door de opdrachtgever een update te geven voor de laatste sprint en een keuze voor bepaalde func met prioretering. Ik ga dit tijdens mijn stage anders aanpakken. Vooral met contact richting de begeleiders toe, door i.i.g. iedere 2 weken een update te geven.
- Wij hebben niet op tijd de images voor de sprint review klaar gezet. Hierdoor werkte alles niet tijdens de review en stonden wij daar met onze mond vol tanden. Dit hebben wij opgelost door op zn minst een dag van te voren de images te builden en naar productie te zetten. Ik wil dit ook meenemen tijdens mijn stage en het werkende leven. Ik kan beter iets nog niet helemaal af en wel kunnen laten zien, dan dat alles in de soep loopt en ik helemaal niks heb.
## 9. Conclusie & feedback

Competenties: *DevOps-7 - Attitude*

Ik vond het een heel uitdagend project, maar wel heel leerzaam. Het domein was heel lastig en het niet kennen van angular hielp ook niet mee. Hierdoor hadden wij een vrij trage start. Ik vond het wel heel leuk om op de devops manier te werken en daarom dus ook heel nuttig dat onze eerste sprint review helemaal fout ging. Hierdoor hebben wij voor de daarop volgende sprint reviews de dag van te voren werkende images naar productie gezet en daar niet meer aangekomen. Verder was ik een erg groot fan van de tech reviews. Hier kwam altijd nuttige feedback uit en de opdrachten waren ook leuk. Alleen had wel iets beter gecommuniceerd mogen worden in het begin wat een tech review in hield. Dit was mij in het begin niet duidelijk

Feedback:
Jaap: Fijne begeleider. Heeft ons met de sprint reviews naar mijn mening veel nuttige inzichten gegeven waar wij zelf niet aan hadden gedacht. Bijvoorbeeld bij het deployen naar productie moeten geen nieuwe images gemaakt worden, maar de werkende van dev overgenomen worden. Alleen jammer dat wij geen tussentijdse beoordeling hebben gehad. Hadden wij ook gedeeltelijk aan ons zelf te danken.

Minor: Leuke en interesante minor. Ik vond het leuk dat de tech review opdrachten op nuttige situaties waren waarvan ik mij ook kan inbeelden dat je dit moet kunnen bij een bedrijf. Vind het wel een beetje lastig dat wij weinig indicatie krijgen tijdens het project hoe wij ervoor staan.
