# Eigen bijdrage Hugo Kemme


## 1. Code/platform bijdrage

Competenties: *DevOps-1 Continuous Delivery*

Dev:
- Ik heb het tabel component dat wij gebruiken generiek gemaakt, zodat wij het op een simpele manier konden hergebruiken. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7664?path=/src/WebApp/src/app/app.component.html).
- Backend voor rantsoen en basisrantsoen opgezet. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7231?path=/src/BasalRation/Controllers/RationController.cs).
- Verwijderen van een basisvoer uit een rantsoen. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7587?path=/src/WebApp/src/app/app.component.html).


Ops:
- Ik heb docker compose opstart problemen opgelost door een health check toe te voegen. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7608?path=/src/WebApp/src/environments/environment.staging.ts).
- Bijdrage aan kubernetes infrastructure bestanden. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/mcps-gitops/pushes?itemVersion=GBmain&userId=5e30f7c5-944f-66a2-88ef-9bf5f0a8d160&userName=Hugo%20Kemme%20(student)).
## 2. Bijdrage app configuratie/containers/kubernetes

Competenties: *DevOps-2 Orchestration, Containerization*

- Ik heb de deployment.yaml en service.yaml voor de BasalRationAPI geschreven. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/mcps-gitops/pushes/104669).
- Toevoegen van de ingress.yaml. Zie [push](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pushes/99766).
- Toevoegen van BasalRation microservice in de docker-compose met dockerfile. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7068?path=/src/docker-compose.yml).
- Het uit elkaar trekken van de "monoliet". Wij hadden aan het begin voor gemak rantsoen en basis rantsoen in een microservice laten staan. Ik heb deze uit elkaar getrokken. Zie [pull-request](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/commit/fd22ee455ea0d21b9ec428af13ccb3d6613f6e8b?refName=refs%2Fheads%2FBE-energy-food-table).

## 3. Bijdrage versiebeheer, CI/CD pipeline en/of monitoring

Competenties: *DevOps-1 - Continuous Delivery*, *DevOps-3 GitOps*, *DevOps-5 - SlackOps*

- Het maken van bouwen en pushen van images in de pipeline. Zie [push](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pushes/100096).


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
- [Review](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7881) op code.
- [Review](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7909) op code.
## 6. Bijdrage documentatie

Competenties: *DevOps-6 Onderzoek*

- Zoals eerder al gemeld, heb ik het domeinmodel opgesteld.
- Verder hebben wij gezamelijk alle userstories gemaakt en uitgewerkt.
- Ik heb [adr-2.md](../docs/adr/adr-2.md) over EF Core geschreven.


## 7. Bijdrage Agile werken, groepsproces, communicatie opdrachtgever en soft skills

Competenties: *DevOps-1 - Continuous Delivery*, *Agile*

- Ik heb samen met Osama deelgenomen aan alle Q&A's.
- Ik heb de sprint 2 sprint planning geleid.
- Ik heb de sprint 1 review gepresenteerd.
- Ik ben tijdens een aantal DSU's de scrum master geweest.

## 8. Leerervaringen

Competenties: *DevOps-7 - Attitude*

Tops:
- Dit is het eerste project voor mij dat we vanuit huis maken. Uit mijn ervaring met stage had ik met thuiswerken moeite met concentreren. Ik heb dat tijdens dit project niet en besteed mijn tijd nuttig.
- Ik ben goed bereikbaar en meteen instaat om met iemand te bellen als die hulp nodig heeft.

Tips:
- Beter reviewen. Ik review vaak code niet goed genoeg, omdat de functionaliteit gemerged moet worden voor bijvoorbeeld een sprint review.
- Wij hebben nu vooral Osama aan de pipeline laten bouwen, omdat wij het idee hadden dat het te veel tijd zou kosten als we het zouden afwisselen. Dit heeft er toe geleid dat wij allemaal weinig tot niks kunnen aantonen voor bijdrage aan de pipeline en Osama ons alsnog moet uitleggen wat hij heeft gedaan.
- Ik heb in de eerste sprint veel te lang vast gezeten op de frontend, omdat angular nieuw voor mij was. Dit heeft mij onnodig veel tijd gekost en ik had eerder aan de bel moeten trekken.
## 9. Conclusie & feedback

Competenties: *DevOps-7 - Attitude*

Ik vind het een behoorlijk uitdagend project. Dit heeft vooral te maken met het lastige domein en een nieuw frontend framework. Ik merk dat wij het lastig vinden om de principes van devops goed toe te passen, omdat wij graag functionaliteit willen neerzetten voor de opdrachtgever. Hierdoor zijn wij niet goed bezig met continuous deployment (vooral naar productie) en hebben wij nog amper naar grafana kunnen kijken.
