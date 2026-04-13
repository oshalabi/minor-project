# Eigen bijdragen Osama Halabi

## Samenvatting van mijn Bijdrage

Tijdens het project heb ik in mijn rol als Developer en Ops-er bijgedragen aan verschillende onderdelen van de applicatie en infrastructuur, waarbij ik me richtte op code-ontwikkeling, configuratie en documentatie. Hieronder geef ik een kort overzicht van mijn belangrijkste bijdragen:

- **Microservices en Functionaliteit:** Ontwikkeling en implementatie van nieuwe functionaliteiten binnen de microservices-architectuur.
- **Configuratie en Containerization:** Optimalisatie van configuraties, het opzetten van Docker-containers en gebruik van Kubernetes voor schaalbare implementaties
- **Documentatie en Diagrammen:**
 Uitgebreide documentatie van workflows, pipelines en architectuurdiagrammen om teamleden en stakeholders beter te informeren
- **Scrum en Teamprocessen:** Actieve deelname aan Agile/Scrum-ceremonies, inclusief planning, retrospectieve en communicatie met de product owner.


Door mijn bijdrage heb ik mijn technische expertise in DevOps aanzienlijk verdiept en mijn samenwerking- en communicatievaardigheden verder ontwikkeld, wat heeft bijgedragen aan de voortgang en kwaliteit van het project.

## 1. Code/platform bijdrage

Competenties: *DevOps-1 Continuous Delivery*

In mijn rol als [Developer en Ops-er] heb ik verschillende bijdragen geleverd binnen het project. Hieronder beschrijf ik mijn belangrijkste bijdragen:

### Bijdragen voor Developer (Dev)

Als Developer heb ik bijgedragen aan de volgende functionaliteiten:

1. **Bijdragen 1**
   * Beschrijving: Ik heb deze user story geïmplementeerd [Als veevoeradviseur wil ik dat de totale voedselwaarden worden berekend, wanneer ik een voedselwaarde heb aangepast, zodat ik direct kan zien wat voor een effect mijn aanpassing heeft op het rantsoen](https://dev.azure.com/ISMinor/HANMinorGroep5/_sprints/taskboard/HANMinorGroep5%20Team/HANMinorGroep5/Iteration%203?workitem=5633)
   * Link naar code: [PR-1](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7881), [PR-2](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7942), [PR-3](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8015)

2. **Bijdragen 2**
   * Beschrijving: In deze PR heb ik de ration controller herschreven voor beter testing en hergebruiken van functies daarnaast heb ik  ook duplicatie verminderd.
   * Link naar code: [PR](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8203)

3. **Bijdragen 3**
   * Beschrijving: In deze PR heb ik het berekenen van voor zowel de backend als de frontend van het totaalregel van de basisrantsoentabel geïmplementeerd.
   * Link naar code: [PR](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7881?_a=files)

4. **Bijdragen 4**
   * Beschrijving: In deze PR heb ik de user story geïmplementeerd voor het maken van een nieuwe rantsoen.
   * Link naar code: [PR](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8222)

5. **Bijdragen 5**
   * Beschrijving: In deze PR heb ik de user story geïmplementeerd voor het maken van een nieuwe rantsoen.
   * Link naar code: [PR](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8222)

## 2. Bijdrage app configuratie/containers/kubernetes

Competenties: *DevOps-2 Orchestration, Containerization*

Mijn belangrijkste bijdragen op het gebied van configuratie en containerization zijn:

1. **Bijdragen 1**
   * **[Docker File](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/commit/258fa6237f41ab743f9311247b522b41d407443c?refName=refs%2Fheads%2Fdevelopment&path=%2Fsrc%2FBasalRation%2FDockerfile&_a=compare):** Beschrijving: Ik heb de docker file voor de basalRation opnieuw geschreven zodat de gebuilde image in de staging- en productieomgeving kan werken.

2. **Bijdragen 2**
   * **[HELM Chart](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/mcps-gitops/commit/f354292c3f10b8651872784afdc444fe690d1aa4?refName=refs/heads/main):** Beschrijving: In onze git-ops repo heb HELM Chart en template toegevoegd om duplicatie te verminderen en het deployen beter en makkelijker te maken.

3. **Bijdragen 3**
   * **[DownTime verminderen](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/mcps-gitops/commit/68b168106cac02490d6fa113253a3ace86239731?refName=refs/heads/main&path=/agrifirm/templates/basalration-deployment.yaml&_a=compare):** Beschrijving: Ik heb strategy, terminationGracePeriodSeconds, readinessProbe, livenessProbe toegevoegd aan alle deployments.yaml om de downtime tijdens het deployen van de applicatie zo laag mogelijk te maken.

4. **Bijdragen 4**
   * **[Skaffold](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7679?_a=files):** Beschrijving: Ik heb Skaffold toegevoegd zodat we zo weinig mogelijk verschil kunnen hebben tussen development, staging, en productie wat een van de The Twelve Factors APP is.

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

Tijdens dit project heb ik een onderzoek gedaan naar een autoversioning methode die we tijdens dit project moeten gaan gebruiken. Uit dit onderzoek gebleken dat er zijn verschillende manieren waar we de Semantic Versioning kunnen toepassen hieronder de gevonden methode kort beschreven. 

#### Moonlit Auto-Versioning
De Moonlit Auto-Versioning-methode automatiseert het proces van versiebeheer op basis van commitberichten en pull request-titels.
#### Semver met Semantic Release
Semantic Release gebruikt een gestandaardiseerde set tools om versienummers te genereren op basis van semantische versiebeheerregels
#### Custom CI/CD Pipelines
Ontwikkelaars kunnen CI/CD-tools zoals Jenkins, GitHub Actions of GitLab CI gebruiken om een op maat gemaakte versiebeheeroplossing te implementeren

Om dit onderozke verder te lezen kan je naar [Auto-Versioning-Onderzoek](../docs/auto-versioning-onderzoek.md)

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

1. **Bijdragen 1** Ik heb de release pipeline beschreven en gedocumenteerd. Dit staat beschreven in het document [release-pipeline](../docs/release-pipeline-doc.md)
   
2. **Bijdragen 2** Ik heb de deployment en build pipeline gedocumenteerd een ook uit gelegd in dit document. [deployment en build pipeline](../docs//deployment-build-pipeline-doc.md)
   
3. **Bijdragen 3** Ik heb een testplan gemaakt en gedocumenteerd. [Testplan](../docs/testplan.md)
4. **Bijdragen 4** Naast al deze documenten heb ik script geschreven die door beide pipelines uitgevoerd wordt en ook gedocumenteerd. [update values script](../docs//update-values-script-doc.md)



## 7. Bijdrage Agile werken, groepsproces, communicatie opdrachtgever en soft skills

Competenties: *DevOps-1 - Continuous Delivery*, *Agile*

Tijdens het project heb ik actief deelgenomen aan Scrum ceremonies en bijgedragen aan zowel het groepsproces als de communicatie met de opdrachtgever. Enkele voorbeelden van mijn inbreng zijn:

1. **Sprint Planning**: Tijdens de sprint planning heb ik actief geholpen bij het inschatten van de benodigde tijd voor verschillende taken en het prioriteren van de backlog-items. Samen met het team heb ik gewerkt aan het opstellen van duidelijke sprintdoelen om een goed gefocuste en haalbare sprint te plannen.

2. **Sprint Reviewe**: Tijdens de sprint hadden we een moment waarop we keken naar hoe onze sprint is verlopen. Hiervoor moesten we de producte presenteren aan alle stakeholders wat we tijdens deze sprint hebben afgerond en wat we van plan zijn voor de volgende sprint. Deze meeting heeft plaats gevonden in Utrecht.

3. **Retrospective**: Tijdens de retrospectives heb ik constructieve feedback gegeven over ons teamproces. Ik heb een aantal verbeterpunten genoemd, zoals het tijdig oppakken en afronden van individuele taken en heldere communicatie over voortgang. Ook heb ik enkele successen benadrukt om het team te motiveren. [Retrospective 1](https://miro.com/app/board/uXjVL-Cgw-Y=/), [Retrospective 2](https://miro.com/app/board/uXjVL4VzFdQ=/)

4. **Po Proxy**: Tijdens dit project was ik de contactpersoon tussen mijn team en de product owner.


Deze activiteiten hebben bijgedragen aan een goede samenwerking, duidelijke communicatie en een betere afstemming binnen het team en met de opdrachtgever.

## 8. Leerervaringen

Competenties: *DevOps-7 - Attitude*

Tot slot, enkele **tops** en **tips** op het gebied van professional skills die ik meeneem voor mijn verdere loopbaan:

### Tops
1. **Samenwerken**: De samenwerking binnen het team verliep soepel en effectief. We hebben elkaar goed ondersteund, wat de productiviteit en teamdynamiek bevorderde.
2. **Behulpzaamheid**: Ik heb actief mijn teamleden geholpen met hun taken wanneer ze ondersteuning nodig hadden. Dit heeft bijgedragen aan een positieve sfeer en een vlotte voortgang van het project.
3. **Effectieve samenwerking**: Door heldere communicatie en een constructieve houding in het team heb ik bijgedragen aan een positieve werksfeer en goede resultaten.
4. **Technische groei**: Het werken in een auzr omgeving en met tools zoals Kubernetes en GitOps heeft mijn technische kennis aanzienlijk verbreed.
5. **Probleemoplossend vermogen**: Ik heb geleerd om technische uitdagingen efficiënt aan te pakken, zoals het verminderen van downtime en het implementeren van autoscaling.
   
### Tips

1. **User stories opdelen**: Ik heb gemerkt dat het opdelen van user stories in kleinere taken zorgt voor meer overzicht en haalbaarheid.
   
2. **Tijdmanagement**: Het tijdig oppakken van taken en het spreiden van werk over de sprint voorkomt last-minute stress.

Daarnaast heb ik tijdens het project waardevolle hulp en feedback ontvangen van mijn groepsgenoten. Eén van de meest opvallende momenten was toen een teamlid me op een constructieve manier feedback gaf over de indeling van mijn user story, wat me hielp mijn aanpak aan te passen en mijn werk effectiever te structureren.

### Transfer naar afstuderen

Deze leerervaringen neem ik mee naar mijn afstudeeropdracht, waarbij ik wil focussen op duidelijke planning, effectieve communicatie en het toepassen van DevOps-principes in een professionele omgeving. Vooral het gebruik van tools zoals Kubernetes en CI/CD-pipelines zal een integraal onderdeel vormen van mijn afstudeerproject.

## 9. Conclusie & feedback

Competenties: *DevOps-7 - Attitude*

Terugkijkend op dit project ben ik tevreden over mijn bijdrage en de geleverde inspanningen. Ik heb veel kunnen leren en bijdragen aan zowel technische als organisatorische aspecten, zoals het implementeren van nieuwe functionaliteiten, het maken van ondersteunende documentatie en diagrammen. Deze taken hebben me geholpen om meer inzicht te krijgen in de complete DevOps-levenscyclus en de samenwerking binnen een team.

Tijdens het project heb ik gemerkt dat heldere communicatie en een goede verdeling van user stories belangrijk zijn voor een soepel verloop. Het werken in een Azure heeft me geholpen mijn DevOps-technieken te verfijnen, en het opzetten van ADR's bood me waardevolle ervaring in het documenteren van beslissingen en architectuur.

Constructieve feedback aan docenten/beoordelaars:
- Het zou waardevol zijn om meer voorbeelden en richtlijnen te krijgen over het documenteren van beslissingen met ADR's in DevOps-projecten, zodat studenten vanaf het begin gestructureerd kunnen werken.
- Daarnaast zou een sessie over het opstellen van effectieve user stories en het opdelen ervan in kleine, beheersbare taken nuttig zijn om overzicht en controle te behouden in grotere projecten.

Met deze opgedane kennis voel ik me goed voorbereid om in toekomstige projecten een actieve bijdrage te leveren aan zowel technische als organisatorische aspecten. De nadruk op communicatie, planning en DevOps-principes zoals continuous delivery zal mijn werkmethodiek verder verbeteren.

Deze ervaringen hebben me geholpen om een sterke basis te leggen voor mijn afstudeeropdracht en om mijn vaardigheden als DevOps-professional verder te ontwikkelen.

