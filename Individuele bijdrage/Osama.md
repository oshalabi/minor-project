# Eigen bijdragen Osama Halabi

## Samenvatting van mijn Bijdrage

Tijdens het project heb ik in mijn rol als Developer en Ops-er bijgedragen aan verschillende onderdelen van de applicatie en infrastructuur, waarbij ik me richtte op code-ontwikkeling, configuratie en documentatie. Hieronder geef ik een kort overzicht van mijn belangrijkste bijdragen:

- **Microservices en Functionaliteit**:

- **Configuratie en Containerization**:


- **Documentatie en Diagrammen**:


- **Scrum en Teamprocessen**:


In deze rollen heb ik zowel mijn DevOps-vaardigheden als mijn samenwerking- en communicatievaardigheden kunnen verbeteren, wat de voortgang en kwaliteit van het project ten goede kwam.

## 1. Code/platform bijdrage

Competenties: *DevOps-1 Continuous Delivery*

In mijn rol als [Developer en Ops-er] heb ik verschillende bijdragen geleverd binnen het project. Hieronder beschrijf ik mijn belangrijkste bijdragen:

### Bijdragen voor Developer (Dev)

Als Developer heb ik bijgedragen aan de volgende functionaliteiten:

1. **Bijdragen 1**
   * Beschrijving: Ik heb deze user story geïmplementeerd [Als veevoeradviseur wil ik dat de totale voedselwaarden worden berekend, wanneer ik een voedselwaarde heb aangepast, zodat ik direct kan zien wat voor een effect mijn aanpassing heeft op het rantsoen](https://dev.azure.com/ISMinor/HANMinorGroep5/_sprints/taskboard/HANMinorGroep5%20Team/HANMinorGroep5/Iteration%203?workitem=5633)
   * Link naar code: [PR-1](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7881), [PR-2](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7942), [PR-3](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8015)

## 2. Bijdrage app configuratie/containers/kubernetes

Competenties: *DevOps-2 Orchestration, Containerization*

Mijn belangrijkste bijdragen op het gebied van configuratie en containerization zijn:

1. **Bijdragen 1**
   * **[Docker File](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/commit/258fa6237f41ab743f9311247b522b41d407443c?refName=refs%2Fheads%2Fdevelopment&path=%2Fsrc%2FBasalRation%2FDockerfile&_a=compare):** Beschrijving: Ik heb de docker file voor de basalRation opnieuw geschreven zodat de gebuilde image in de staging- en productieomgeving kan werken.

2. **Bijdragen 2**
   * **[HELM Chart](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/mcps-gitops/commit/f354292c3f10b8651872784afdc444fe690d1aa4?refName=refs/heads/main):** Beschrijving: In onze git-ops repo heb helm chart en template toegevoegd om duplicatie te verminderen en het deployen beter en makkelijker te maken.

3. **Bijdragen 3**
   * **[DownTime verminderen](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/mcps-gitops/commit/68b168106cac02490d6fa113253a3ace86239731?refName=refs/heads/main&path=/agrifirm/templates/basalration-deployment.yaml&_a=compare):** Beschrijving: Ik heb strategy, terminationGracePeriodSeconds, readinessProbe, livenessProbe toegevoegd aan alle deployments.yaml om de downtime tijdens het deployen van de applicatie zo laag mogelijk te maken.

4. **Bijdragen 4**
   * **[Skaffold](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7679?_a=files):** Beschrijving: Ik heb skaffold toegevoegd zodat we zo weinig mogelijk verschil kunnen hebben tussen development, staging, en productie wat een van de The Twelve Factors APP is.

## 3. Bijdrage versiebeheer, CI/CD pipeline en/of monitoring

Competenties: *DevOps-1 - Continuous Delivery*, *DevOps-3 GitOps*, *DevOps-5 - SlackOps*

1. **Bijdragen 1**
   * **[Versiebeheer](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/commit/1532abc04e8ef8b590687562065a2521d1b32eb1?refName=refs%2Fheads%2Fdevelopment):** Beschrijving: Ik heb een versiebeheer script gemaakt waarmee we het versienummer updatete bij elke fix of nieuwe feature op basis van Semantic versioning Major.Minor.Patch

2. **Bijdragen 2**
   * **[CI Pipeline](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/commit/10c44b0a7908f2401ee2c8fa7542fee0e32503ec?refName=refs%2Fheads%2Fdevelopment):** Beschrijving: Ik heb een pipeline gemaakt waarmee we de applicatie testen en builden voor dat hij gedeployed naar de staging.

3. **Bijdragen 3**
   * **[Update versie in staging and productie](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/mcps-gitops/commit/c207cea4d2880f8642d115bf5115006e643d3e74?refName=refs%2Fheads%2Fmain):** Beschrijving: Ik heb een script geschreven die door de pipeline wordt gebuikt om de nieuwe gebuild en gepushte images naar de staging- en productieomgeving te deployen.

4. **Bijdragen 4**
   * **[Release pipeline](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7532?_a=files):** Beschrijving: Ik heb release pipeline gemaakt waarmee we de geteste applicatie releasen en deployen naar productie.

5. **Bijdragen 5**
   * **[Git Hook](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7423?_a=files):** Beschrijving: Ik heb een git-hook geschreven zodat alle commit messages een bepaalde structuur te hebben die dan later door de pipeline zal worden gebruikt.

## 4. Onderzoek

Competenties: *Nieuwsgierige houding*
**Vraag?**

## 5. Bijdrage code review/kwaliteit anderen en security

Competenties: *DevOps-7 - Attitude*, *DevOps-4 DevSecOps*

Tijdens mijn werkzaamheden binnen DevOps heb ik meerdere code-reviews uitgevoerd, waarbij ik me heb gericht op zowel de kwaliteit van de code als de werking van de CI/CD-pijplijn. Hier zijn de twee belangrijkste review-acties die ik heb uitgevoerd:

1. **Bijdragen 1**
   * Beschrijving: Ik heb de project setup geïnitialiseerd en de start punt gemaakt voor de hele applicatie
   * Link naar code: [commit](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/commit/9cdd45c9e00cdec5fa34a8032abe617fd0d9c31b?refName=refs%2Fheads%2Fdevelopment)

2. **Bijdragen 2**
   * Beschrijving: Ik heb een aantal pull-requests van teamgenoten beoordeeld, wat mij heeft geholpen de kwaliteit van de applicatie te verbeteren.
   * Link naar reviews: [PR-1](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7245), [PR-2](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7231)

Met deze reviews heb ik een actieve bijdrage geleverd aan het verbeteren van de kwaliteit en stabiliteit van de applicatie, evenals de betrouwbaarheid van de CI/CD-pijplijn. Deze acties ondersteunen de DevOps-competenties van *kwaliteit* en *continuïteit* door het implementeren van structurele verbeteringen en probleemoplossingen.

## 6. Bijdrage documentatie

Competenties: *DevOps-6 Onderzoek*
**Nog TODO**


## 7. Bijdrage Agile werken, groepsproces, communicatie opdrachtgever en soft skills

Competenties: *DevOps-1 - Continuous Delivery*, *Agile*

Tijdens het project heb ik actief deelgenomen aan Scrum ceremonies en bijgedragen aan zowel het groepsproces als de communicatie met de opdrachtgever. Enkele voorbeelden van mijn inbreng zijn:

1. **Sprint Planning**: Tijdens de sprint planning heb ik actief geholpen bij het inschatten van de benodigde tijd voor verschillende taken en het prioriteren van de backlog-items. Samen met het team heb ik gewerkt aan het opstellen van duidelijke sprintdoelen om een goed gefocuste en haalbare sprint te plannen.

2. **Daily Standup Meetings**: Om de dagelijkse communicatie en afstemming binnen het team te verbeteren, heb ik de Daily Standup meeting aangemaakt en op 9:30 gepland. Omdat een teamlid niet beschikbaar was op dit tijdstip, heb ik het tijdstip in overleg met het team verplaatst naar 10:00, zodat iedereen kon deelnemen.

3. **Retrospective**: Tijdens de retrospectives heb ik constructieve feedback gegeven over ons teamproces. Ik heb een aantal verbeterpunten genoemd, zoals het tijdig oppakken en afronden van individuele taken en heldere communicatie over voortgang. Ook heb ik enkele successen benadrukt om het team te motiveren. [Retrospective 1](https://miro.com/app/board/uXjVL-Cgw-Y=/), [Retrospective 2](https://miro.com/app/board/uXjVL4VzFdQ=/)

4. **Po Proxy**: Tijdens dit project was ik de contactpersoon tussen mijn team en de product owner.


Deze activiteiten hebben bijgedragen aan een goede samenwerking, duidelijke communicatie en een betere afstemming binnen het team en met de opdrachtgever.

## 8. Leerervaringen

Competenties: *DevOps-7 - Attitude*

Tot slot, enkele **tops** en **tips** op het gebied van professional skills die ik meeneem voor mijn verdere loopbaan:

### Tops
1. **Samenwerken**: De samenwerking binnen het team verliep soepel en effectief. We hebben elkaar goed ondersteund, wat de productiviteit en teamdynamiek bevorderde.
2. **Behulpzaamheid**: Ik heb actief mijn teamleden geholpen met hun taken wanneer ze ondersteuning nodig hadden. Dit heeft bijgedragen aan een positieve sfeer en een vlotte voortgang van het project.

### Tips
1. **User Stories beter verdelen**: Ik heb geleerd dat ik mijn user stories beter kan opdelen in kleinere, beheersbare taken. Dit zal de voortgang en overzichtelijkheid van mijn werk verbeteren.
2. **Sprinttaken tijdig oppakken**: Een ander aandachtspunt is om mijn taken eerder in de sprint op te pakken. Dit voorkomt last-minute stress en zorgt voor meer spreiding van werk.

Daarnaast heb ik tijdens het project waardevolle hulp en feedback ontvangen van mijn groepsgenoten. Eén van de meest opvallende momenten was toen een teamlid me op een constructieve manier feedback gaf over de indeling van mijn user story, wat me hielp mijn aanpak aan te passen en mijn werk effectiever te structureren.

## 9. Conclusie & feedback

Competenties: *DevOps-7 - Attitude*

Terugkijkend op dit project ben ik tevreden over mijn bijdrage en de geleverde inspanningen. Ik heb veel kunnen leren en bijdragen aan zowel technische als organisatorische aspecten, zoals het implementeren van nieuwe functionaliteiten, het opzetten van configuraties voor autoscaling met KEDA, en het maken van ondersteunende documentatie en diagrammen. Deze taken hebben me geholpen om meer inzicht te krijgen in de complete DevOps-levenscyclus en de samenwerking binnen een team.

Tijdens het project heb ik gemerkt dat heldere communicatie en een goede verdeling van user stories belangrijk zijn voor een soepel verloop. Het werken met KEDA en Kubernetes heeft me geholpen mijn DevOps-technieken te verfijnen, en het opzetten van een C4 Model en ADR's bood me waardevolle ervaring in het documenteren van beslissingen en architectuur.

Constructieve feedback aan docenten/beoordelaars:
- Het zou waardevol zijn om meer voorbeelden en richtlijnen te krijgen over het documenteren van beslissingen met ADR's in DevOps-projecten, zodat studenten vanaf het begin gestructureerd kunnen werken.
- Daarnaast zou een sessie over het opstellen van effectieve user stories en het opdelen ervan in kleine, beheersbare taken nuttig zijn om overzicht en controle te behouden in grotere projecten.

Deze opgedane kennis en vaardigheden zal ik meenemen naar toekomstige DevOps-projecten en mijn verdere loopbaan, waarin ik vooral de waarde van heldere communicatie, documentatie, en continue verbetering van de DevOps-pijplijn meeneem. Met deze ervaring voel ik me beter voorbereid op mijn afstudeeropdracht en ben ik ervan overtuigd dat ik de geleerde technieken kan toepassen in een professionele werkomgeving.