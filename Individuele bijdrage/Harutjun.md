# Eigen bijdrage Harutjun

In dit document beschrijf ik mijn individuele bijdrage aan het beroepsproduct voor dit project. Het doel van dit verslag is om inzicht te geven in de specifieke taken en verantwoordelijkheden die ik heb opgepakt en hoe deze bijdragen aan het totaalresultaat. Ik licht hierbij mijn werkzaamheden toe aan de hand van concrete voorbeelden, met linkjes naar relevante code, documentatie, pull requests en andere bronnen.

Dit document is gestructureerd in verschillende hoofdstukken, waarbij elke sectie een ander aspect van mijn bijdrage bespreekt, zoals mijn werk aan de code, configuratie van de applicatie, versiebeheer, onderzoek, en samenwerking binnen het team. Tot slot reflecteer ik op mijn leerervaringen en geef ik een korte conclusie en eventuele feedback om zowel mijn eigen groei als de kwaliteit van het project verder te bevorderen.

## 1. Code/platform bijdrage

Competenties: *DevOps-1 Continuous Delivery*

In de eerste sprint heb ik vooral aan de frontend gewerkt en heb ik als eerst de frontend met de backend gekoppeld bij het maken van het basisrantsoen tabel. Daarnaast heb ik voor dat tabel het ophalen en weergeven van de eettypen uit de backend gerealiseerd en het toevoegen van voertypen. Ik heb het niet alleen bij frontend gelaten en ben ook gaan werken aan de backend. Zo heb ik de hele microservice van de krachtvoer gemaakt met daarbij ook de endpoints voor de grafiek die ik ook gerealiseerd heb. Ook heb ik de modal gemaakt voor het invoeren van de min en max brokgift, met de daarbij behorende backend code.

- [Eerst frontend merge voor andere om mee door te gaan.](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7254?_a=files)
- [Opgezet microservice krachtvoer](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7954?_a=files)
- [Toevoegen voertype aan basisrantsoen gerealiseerd](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7566?_a=files)
- [Toevoegen mogelijkheid wijzigen min en max brokgift](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8731?_a=files)

## 2. Bijdrage app configuratie/containers/kubernetes

Competenties: *DevOps-2 Orchestration, Containerization*

Voor dit onderdeel heb ik de initiële kubvernetes omgeving opgezet, ik was namelijk ook de enige die was gegaan naar de eerste workshop van het project die werd gegeven door infosupport. Ook heb ik bij het maken van de microservice daarbij de benodigde dockerfile gemaakt en de `docker-compose.yml` bijgewerkt om deze service toe te voegen bij de applicatie. Tijdens dit proces heb ik ook gecontroleerd of de service correct draaide in een containerized omgeving.

- Testen van lokale Docker-container om errors te valideren.
- [Krachtvoer microservice dockerfile en docker compose file gemaakt en geupdate.](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7954?_a=files)
- [Initiële commits voor setup kubernetes omgeving](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/mcps-gitops/commits?user=H.Harutjunjan&userId=7a9c0cf6-78e6-6ebd-b6e3-907ea3a05b5d)

## 3. Bijdrage versiebeheer, CI/CD pipeline en/of monitoring

Competenties: *DevOps-1 - Continuous Delivery*, *DevOps-3 GitOps*, *DevOps-5 - SlackOps*

Ik heb de initiele opzet gedaan bij de workshop die gegeven werd door infosupport. Dit was nodig voor het krijgen van inzicht in Grafana. Deze heb ik vervolgens doorgegeven aan mijn teamgenoten. Daarnaast heb ik ook een review comment bij een githook.

- Doorgeven kennis team
- [Opzet eerste kubernetes omgeving voor Grafana](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/mcps-gitops/commits?user=H.Harutjunjan&userId=7a9c0cf6-78e6-6ebd-b6e3-907ea3a05b5d)
- [Kleine bijdrage githook](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7423?_a=files)
- [Opgezet metric totale requests aan webiste](https://grafana.han5.minor.infosupport.dev/d/deah74qwhv5s0f/aantal-http-requests?orgId=1&from=now-30m&to=now&timezone=Europe%2FAmsterdam&editPanel=1)

## 4. Onderzoek

Competenties: *Nieuwsgierige houding*

Voor dit project hebben we geen onderzoeken geschreven. Persoonlijk heb ik mezelf wel verdiept in het gebruik van chartjs voor de grafieken in het krachtvoertabel.

- [ADR gebruik chartjs](/docs/adr/adr-chartjs.md)
- [Gebruikte website voor info](https://www.chartjs.org/)

## 5. Bijdrage code review/kwaliteit anderen en security

Competenties: *DevOps-7 - Attitude*, *DevOps-4 DevSecOps*

In de eerste link die ik geplaatst heb, heb ik wat inhoudelijke code kwaliteit comments geplaatst. Daarnaast heb ik ook een comment op een githook die ik heb geplaatst in de tweede link. In de derde link heb ik een comment geplaatst over de inefficienty van een database-query en hoe dit beteer kan.

- [Feedback implementatie bereken feature](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7942?_a=files&path=/src/WebApp/src/app/basalration/basalration.component.ts&discussionId=62421)
- [Comment githook](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7423)
- [Comment database-query](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullRequest/8374#1736330293)

## 6. Bijdrage documentatie

Competenties: *DevOps-6 Onderzoek*

Ik zal voor ook dit onderwerp de ADR erbij plaatsen, want ik heb dit bij beide hoofdstuk 4 en 6 gebruikt. Ik heb ook bij het maken van de branches de issue nummer ervoor geplaatst en de issue gelinked met de branche. Daarnaast heb ik ook bij user stories goede descripties toegevoegd om zo de opdracht te verduidelijken.

- [ADR gebruik chartjs](/docs/adr/adr-chartjs.md)
- [Benamingen branches](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/branches)
- [Uitgebreide descriptie user story](https://dev.azure.com/ISMinor/HANMinorGroep5/_backlogs/backlog/HANMinorGroep5%20Team/Stories?showParents=true&workitem=5641)

## 7. Bijdrage Agile werken, groepsproces, communicatie opdrachtgever en soft skills

Competenties: *DevOps-1 - Continuous Delivery*, *Agile*

Bij de eerste retrospective was ik degene die leiding nam in het nemen van het woord. Daarnaast ben ik altijd goed aanwezig bij de daily standups en neem is ook best vaak leiding daarin. Bij de sprint review van sprint 2 nam ik goed het woord bij het tonen van het grafiek. Bij sprint review 3 heb ik het gesprek geleid en stond ik achter de laptop om dit te doen.

- [eiden retrospective 1](https://miro.com/app/board/uXjVL-Cgw-Y=/)
- Altijd aanwezig bij DSU's
- Goed bijdrage gehad gesprek sprint review 2
- Sprint review 3 heb ik geleid

## 8. Leerervaringen

Competenties: *DevOps-7 - Attitude*

Tijdens dit project heb ik waardevolle vaardigheden en inzichten opgedaan die ik mee kan nemen naar toekomstige projecten en mijn verdere carrière. Hieronder geef ik een kort overzicht van mijn belangrijkste leerervaringen:

Tops:

- Ik heb tot nu toe al best wat ervaring met C# opgedaan. **Transfer**: die ik met C# heb ontwikkeld, kan ik toepassen in toekomstige backend-ontwikkelingsprojecten. Bovendien biedt deze ervaring een solide basis voor het werken met andere objectgeoriënteerde talen zoals Java of Python.
- Best wat ervaring met Angular opgedaan. **Transfer**: De kennis en vaardigheden die ik met Angular heb opgedaan, kan ik toepassen in toekomstige loopbaan en mijn afstudeerstage, maar ook in andere frontend-frameworks. Angular biedt een goede basis die bruikbaar is in veel verschillende frontend-omgevingen.
- Goed inzicht gekregen op kubernetes. **Transfer**: Voor mijn afstudeerstage wil ik deze kennis toepassen door zelfstandig een Kubernetes-opzet te maken en deze verder te onderhouden. Ik wil ook refereren naar mijn ervaringen uit dit project om te leren hoe ik de omgeving beter kan verbeteren en onderhouden.

Tips:

- Ik heb niet het gevoel dat ik continuous getest heb. **Transfer**:  In toekomstige projecten wil ik vanaf het begin een duidelijke teststrategie opzetten en gebruik maken van tools voor continuous testing, zoals een CI/CD-pipeline met geïntegreerde teststappen. Dit zal mij helpen om stabielere software te ontwikkelen en sneller feedback op wijzigingen te krijgen.
- Ik wil meenemen om volgende keren meer te werken aan het onderhouden en verbeteren van het project. **Transfer**: In toekomstige projecten wil ik actief betrokken blijven bij het volledige proces, inclusief het optimaliseren en doorontwikkelen van de opzet. Dit is een belangrijk leerpunt dat ik zeker zal toepassen tijdens mijn afstudeerstage en verdere loopbaan.
- Sneller en beter inzicht krijgen als het proces niet goed gaat voor opleveren. **Transfer**: In toekomstige projecten wil ik regelmatig overlegmomenten inplannen met begeleiders en opdrachtgevers om de voortgang te bespreken en eventuele knelpunten tijdig te signaleren. Dit zal bijdragen aan een soepelere samenwerking en een beter eindresultaat, vooral bij complexe projecten zoals mijn afstudeerstage.

## 9. Conclusie & feedback

Competenties: *DevOps-7 - Attitude*

Ik kijk met een positief gevoel terug naar het project. Het is een interessant en leerzaam proces geweest, mede omdat het project een realistisch domein heeft, ook al kan het soms uitdagend zijn. Die realistische aspecten maken het echter des te waardevoller om aan te werken.

Echter, één punt van kritiek dat ik wil aanstippen, is dat het lijkt alsof Agrifirm niet altijd alle casusbestanden levert, tenzij hier specifiek om wordt gevraagd. Dit is opmerkelijk, omdat deze bestanden vaak belangrijke informatie bevatten die essentieel zijn om het domein goed te begrijpen. Het risico hiervan is dat studenten mogelijk met een verkeerd beeld van het domein werken. Hoewel ik begrijp dat dit wellicht bedoeld is om studenten aan te moedigen om vragen te stellen, vind ik dit geen correcte aanpak. Het domein is al complex genoeg, en dergelijke obstakels kunnen het proces onnodig verstoren.

Wat betreft feedback voor de docenten en beoordelaars: het zou helpen als er een centrale plek wordt gecreëerd waar alle casusbestanden en relevante informatie overzichtelijk beschikbaar zijn. Dit zou de efficiëntie en het overzicht aanzienlijk verbeteren.

De DevOps-kennis en vaardigheden die ik tijdens dit project heb opgedaan, zullen zeker van pas komen in mijn verdere loopbaan. Binnenkort start ik met een afstudeerstage waarbij ik ook zal moeten integreren met een DevOps-omgeving. De inzichten die ik heb opgedaan, zoals het werken met DevOps-methodieken en tooling en het belang van goede samenwerking in multidisciplinaire teams, vormen hiervoor een stevige basis.

Ik kijk met tevredenheid terug op dit project en ben dankbaar voor de kansen om mezelf verder te ontwikkelen binnen een uitdagend en realistisch domein.
